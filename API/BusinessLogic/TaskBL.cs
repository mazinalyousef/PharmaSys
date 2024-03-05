using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Enumerations;
using API.Helpers;
using API.Hubs;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.BusinessLogic
{
    public class TaskBL : ITaskBL
    {


        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;

         private readonly IHubContext<NotificationHub> _notificationHub;
    
        public TaskBL(
            DataContext dataContext,
          
        UserManager<User> userManager,
         
           IHubContext<NotificationHub> notificationHub
        
        )
        {
            _dataContext=dataContext;
          
            _userManager = userManager;
           
            _notificationHub=notificationHub;
        }




          

        // must change to sync ....
        public bool SetAsCompleted(int _taskId)
        {

             bool isCompleted=false;
             string newTaskMessage="you've received a new task";

              var Production_Manufacturingusers= _userManager.GetUsersInRoleAsync(UserRoles.Production_Manufacturing).GetAwaiter().GetResult();
               var QA_Samplingusers= _userManager.GetUsersInRoleAsync(UserRoles.QA_Sampling).GetAwaiter().GetResult();

                 var Filling_CheckEquipementsusers= _userManager. GetUsersInRoleAsync(UserRoles.Filling_CheckEquipements).GetAwaiter().GetResult();
                  var Filling_FillingTubesUsers= _userManager.GetUsersInRoleAsync(UserRoles.Filling_FillingTubes).GetAwaiter().GetResult();
                   var Filling_CartooningUsers= _userManager.GetUsersInRoleAsync(UserRoles.Filling_Cartooning).GetAwaiter().GetResult();
                   var Filling_PackagingUsers= _userManager. GetUsersInRoleAsync(UserRoles.Filling_Packaging).GetAwaiter().GetResult();
                   var ManagerUsers= _userManager.GetUsersInRoleAsync(UserRoles.Manager).GetAwaiter().GetResult();


                bool notifyProduction_Manufacturing=false;
                bool notifyQA_Sampling=false;
                bool notifyFilling_CheckEquipements=false;
                bool notifyFilling_FillingTubes=false;
                bool notifyFilling_Cartooning=false;
                bool notifyFilling_Packaging=false;
                bool notifyManager=false;

           // wrap in one transaction later...
           // using (IDbContextTransaction)
           // update task  info 
              using (IDbContextTransaction transaction=  _dataContext.Database.BeginTransaction())
              {
                    try 
                    { 
                        var  originalTask=_dataContext.BatchTasks.FirstOrDefault(x=>x.Id==_taskId);
                        bool completed =  false;
                         // set task as completed...
                          // setting new properties for the original entity
                             originalTask.EndDate =DateTime.Now;
                             originalTask.TaskStateId = (int) Enumerations.TaskStatesEnum.finished;
                            _dataContext.Entry(originalTask).Property(x=>x.EndDate).IsModified=true;
                            _dataContext.Entry(originalTask).Property(x=>x.TaskStateId).IsModified=true;
                             _dataContext.SaveChanges();
                             completed=true;
                            // get batch info  , will use later
                            var originalBatch = _dataContext.Batches.FirstOrDefault(x=>x.Id==originalTask.BatchId);

                             // do this without locking for now ...
                            // todo : lock the current code later....

                             #region  pre-production Tasks...
            // check if the current task is pre-production task
            if (IsPreProductionTask(originalTask.TaskTypeId,(int)originalTask.DepartmentId))
            {
                // lock here....

                // check the   state of the batch (from database)...
                
                    var originalbatch=_dataContext.Batches.Where(x=>x.Id==originalTask.BatchId).FirstOrDefault();
                    int batchstate = originalBatch.BatchStateId;
                if (batchstate!=(int)BatchStatesEnum.preProductionCompleted)
                {
                    // check if all other pre-production  tasks (except this task) is completed.....
                     bool isAllPreProductionTasksCompleted = IsAllPreProductionTasksCompleted(_dataContext,originalTask.BatchId,originalTask.Id);
                         if (isAllPreProductionTasksCompleted)
                         {
                          // set Batch new State to pre-production completed....
                          //_batchRepository.setBatchState(originalTask.BatchId,(int)BatchStatesEnum.preProductionCompleted);
                          //  var originalBatch = _dataContext.Batches.FirstOrDefault(x=>x.Id==originalTask.BatchId);
                           originalBatch.BatchStateId=(int)BatchStatesEnum.preProductionCompleted;
                          _dataContext.Entry(originalBatch).Property(x=>x.BatchStateId).IsModified=true;
                          _dataContext.SaveChanges();


                          // add new notifications (to manufacturing - production)...
                           
                              // need to get the Production_Manufacturing task Id for the batch
                              /*
                             BatchTask batchTask= _taskRepository.GetBatchTask(originalTask.BatchId,
                                        (int) TaskTypesEnum.Manufacturing,(int) DepartmentsEnum.Production
                                        );
                                        */


                                         BatchTask batchTask= new BatchTask();
                                           batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==(int) TaskTypesEnum.Manufacturing
                                          &&x.BatchId==originalTask.BatchId
                                          &&x.DepartmentId==(int) DepartmentsEnum.Production).FirstOrDefault();
            
                                foreach (var userItem in Production_Manufacturingusers)
                                {
                                        var userId = userItem.Id;
                                        Notification notification = new Notification ();
                                        notification.BatchId = originalTask.BatchId;
                                        notification.BatchTaskId =batchTask.Id;
                                        notification.DateSent = DateTime.Now;

                                        // need to get the batch number 
                                        notification.NotificationMessage = string.Format("Batch {0} With Manufacturing Task Is Available",
                                        originalBatch.BatchNO.ToString());
                                        notification.UserId = userId;
                                       

                                        _dataContext.Notifications.Add(notification);
                                        _dataContext.SaveChanges(); 
            
                                }


                                notifyProduction_Manufacturing=true;




                               


                         }
                }
            
            }
            #endregion

              #region  Production Tasks.....
            else  // not pre- production task
            {
                 // check if this  is a manufacturing -- production task 
                 if (originalTask.TaskTypeId==(int)TaskTypesEnum.Manufacturing&&originalTask.DepartmentId==(int) DepartmentsEnum.Production)
                 {
                        // add notifications to : (sampling--qa,equipements&machines--filling)

                             // sampling --QA
                            
                             // need to get the Sampling task Id for the batch
                                    //  BatchTask batchTask= _taskRepository.GetBatchTask(originalTask.BatchId,
                                     // (int) TaskTypesEnum.Sampling,(int) DepartmentsEnum.QA
                                     // );

                                      	   BatchTask batchTask= new BatchTask();
                                           batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==(int) TaskTypesEnum.Sampling
                                          &&x.BatchId==originalTask.BatchId
                                          &&x.DepartmentId==(int) DepartmentsEnum.QA).FirstOrDefault();
                              foreach (var userItem in QA_Samplingusers)
                              {
                                     var userId = userItem.Id;
                                      Notification notification = new Notification ();
                                      notification.BatchId = originalTask.BatchId;
                                      notification.BatchTaskId =batchTask.Id;
                                      notification.DateSent = DateTime.Now;
                                      // need to get the batch nomber 
                                     notification.NotificationMessage = string.Format("Batch {0} With Sampling Task Is Available",
                                      originalBatch.BatchNO.ToString());
                                     notification.UserId = userId;

                                       _dataContext.Notifications.Add(notification);
                                          _dataContext.SaveChanges(); 
                                  //  _notificationRepository.Add(notification).GetAwaiter().GetResult();
                              }
                               notifyQA_Sampling=true;


                               // Filling --Check Equipements
                            
                             // need to get the Filling equipements task Id for the batch
                                   //    batchTask= _taskRepository.GetBatchTask(originalTask.BatchId,
                                    //  (int) TaskTypesEnum.Equipments_Machines,(int) DepartmentsEnum.Filling
                                   //   );


                                           batchTask= new BatchTask();
                                           batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==(int) TaskTypesEnum.Equipments_Machines
                                          &&x.BatchId==originalTask.BatchId
                                          &&x.DepartmentId==(int) DepartmentsEnum.Filling).FirstOrDefault();


                              foreach (var userItem in Filling_CheckEquipementsusers)
                              {
                                     var userId = userItem.Id;
                                      Notification notification = new Notification ();
                                      notification.BatchId = originalTask.BatchId;
                                      notification.BatchTaskId =batchTask.Id;
                                      notification.DateSent = DateTime.Now;
                                      // need to get the batch nomber 
                                     notification.NotificationMessage = string.Format("Batch {0} With Check Equipements And Machines Task Is Available",
                                      originalBatch.BatchNO.ToString());
                                     notification.UserId = userId;


                                  //  _notificationRepository.Add(notification).GetAwaiter().GetResult();
                                    _dataContext.Notifications.Add(notification);
                                   _dataContext.SaveChanges(); 
                              }
                              notifyFilling_CheckEquipements=true;
                             
                 }
                 else if (IsPreFillingTask(originalTask.TaskTypeId,(int)originalTask.DepartmentId))
                 {
                // lock here....

                // check the   state of the batch (from database)...
                int batchstate=  originalBatch.BatchStateId;
                if (batchstate!=(int)BatchStatesEnum.preFillingCompleted)
                {
                    // check if all other pre-filling  tasks (except this task) is completed.....
                     bool isAllPreFillingTasksCompleted = IsAllPreFillingTasksCompleted(_dataContext, originalTask.BatchId,originalTask.Id);
                         if (isAllPreFillingTasksCompleted)
                         {
                          // set Batch new State to pre-filling completed....
                          //_batchRepository.setBatchState(originalTask.BatchId,(int)BatchStatesEnum.preFillingCompleted);
                           originalBatch.BatchStateId=(int)BatchStatesEnum.preFillingCompleted;
                           _dataContext.Entry(originalBatch).Property(x=>x.BatchStateId).IsModified=true;
                          _dataContext.SaveChanges();
                          // add new notifications (to Filling--filling tubes)...
                           
                              // need to get the fillingtubes-filling task Id for the batch
                        //     BatchTask batchTask= _taskRepository.GetBatchTask(originalTask.BatchId,
                                      //  (int) TaskTypesEnum.FillingTubes,(int) DepartmentsEnum.Filling
                                     //   );


                                        BatchTask batchTask= new BatchTask();
                                           batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==(int) TaskTypesEnum.FillingTubes
                                          &&x.BatchId==originalTask.BatchId
                                          &&x.DepartmentId==(int) DepartmentsEnum.Filling).FirstOrDefault();

                                foreach (var userItem in Filling_FillingTubesUsers)
                                {
                                        var userId = userItem.Id;
                                        Notification notification = new Notification ();
                                        notification.BatchId = originalTask.BatchId;
                                        notification.BatchTaskId =batchTask.Id;
                                        notification.DateSent = DateTime.Now;

                                        // need to get the batch nomber 
                                        notification.NotificationMessage = string.Format("Batch {0} With Filling Tubes Task Is Available",
                                        originalBatch.BatchNO.ToString());
                                        notification.UserId = userId;
                                     //   _notificationRepository.Add(notification).GetAwaiter().GetResult();
                                       _dataContext.Notifications.Add(notification);
                                          _dataContext.SaveChanges(); 

                                }
                                notifyFilling_FillingTubes=true;
                               

                         }
                }
            
                }
                 else  if (originalTask.TaskTypeId==(int)TaskTypesEnum.FillingTubes&&originalTask.DepartmentId==(int) DepartmentsEnum.Filling)
                 {
                           // add notifications to : (filling--carttoning)
                               
                             
                             // need to get the Filling cartooning task Id for the batch
                                  //   BatchTask batchTask= _taskRepository.GetBatchTask(originalTask.BatchId,
                                    //  (int) TaskTypesEnum.Cartooning,(int) DepartmentsEnum.Filling
                                    //  );

                                         BatchTask batchTask= new BatchTask();
                                           batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==(int) TaskTypesEnum.Cartooning
                                          &&x.BatchId==originalTask.BatchId
                                          &&x.DepartmentId==(int) DepartmentsEnum.Filling).FirstOrDefault();
                              foreach (var userItem in Filling_CartooningUsers)
                              {
                                     var userId = userItem.Id;
                                      Notification notification = new Notification ();
                                      notification.BatchId = originalTask.BatchId;
                                      notification.BatchTaskId =batchTask.Id;
                                      notification.DateSent = DateTime.Now;
                                      // need to get the batch nomber 
                                     notification.NotificationMessage = string.Format("Batch {0} With Cartooning Task Is Available",
                                      originalBatch.BatchNO.ToString());
                                     notification.UserId = userId;
                                    //_notificationRepository.Add(notification).GetAwaiter().GetResult();
                                    _dataContext.Notifications.Add(notification);
                                          _dataContext.SaveChanges(); 
                              }

                              notifyFilling_Cartooning=true;

                             
                 }
                 else  if (originalTask.TaskTypeId==(int)TaskTypesEnum.Cartooning&&originalTask.DepartmentId==(int) DepartmentsEnum.Filling)
                 {
                           // add notifications to : (filling--packaging)
                               
                            
                             // need to get the Filling cartooning task Id for the batch
                                  //   BatchTask batchTask= _taskRepository.GetBatchTask(originalTask.BatchId,
                                   //   (int) TaskTypesEnum.Packaging,(int) DepartmentsEnum.Filling
                                   //   );

                                      BatchTask batchTask= new BatchTask();
                                           batchTask= _dataContext.BatchTasks.Where(x=>x.TaskTypeId==(int) TaskTypesEnum.Packaging
                                          &&x.BatchId==originalTask.BatchId
                                          &&x.DepartmentId==(int) DepartmentsEnum.Filling).FirstOrDefault();
                              foreach (var userItem in Filling_PackagingUsers)
                              {
                                     var userId = userItem.Id;
                                      Notification notification = new Notification ();
                                      notification.BatchId = originalTask.BatchId;
                                      notification.BatchTaskId =batchTask.Id;
                                      notification.DateSent = DateTime.Now;
                                      // need to get the batch nomber 
                                     notification.NotificationMessage = string.Format("Batch {0} With Packaging Task Is Available",
                                      originalBatch.BatchNO.ToString());
                                     notification.UserId = userId;
                                 //   _notificationRepository.Add(notification).GetAwaiter().GetResult();
                                  _dataContext.Notifications.Add(notification);
                                          _dataContext.SaveChanges(); 
                              }

                              notifyFilling_Packaging=true;
                            
                 }
                 else  if (originalTask.TaskTypeId==(int)TaskTypesEnum.Packaging&&originalTask.DepartmentId==(int) DepartmentsEnum.Filling)
                 {

                             //  update batch info ....
                              // set Batch new State to  completed....
                           // _batchRepository.setBatchState(originalTask.BatchId,(int)BatchStatesEnum.finished);
                           // add notifications to : (Manager)
                                 originalBatch.BatchStateId=(int)BatchStatesEnum.finished;
                             _dataContext.Entry(originalBatch).Property(x=>x.BatchStateId).IsModified=true;
                             _dataContext.SaveChanges();
                           
                             
                                     
                              foreach (var userItem in ManagerUsers)
                              {
                                     var userId = userItem.Id;
                                      Notification notification = new Notification ();
                                      notification.BatchId = originalTask.BatchId;
                                      notification.BatchTaskId =null;
                                      notification.DateSent = DateTime.Now;
                                      // need to get the batch nomber 
                                     notification.NotificationMessage = string.Format("Batch {0} Has Completed...",
                                      originalBatch.BatchNO.ToString());
                                     notification.UserId = userId;
                                  //  _notificationRepository.Add(notification).GetAwaiter().GetResult();
                                   _dataContext.Notifications.Add(notification);
                                          _dataContext.SaveChanges(); 
                              }
                              notifyManager=true;
                             
                 }
                 

            }
            #endregion

                        transaction.Commit();
                        isCompleted=true;


                        #region notifications
                        if (notifyProduction_Manufacturing)
                        {
                               _notificationHub.Clients.Group(UserRoles.Production_Manufacturing).
                                SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();

                                _notificationHub.Clients.Group(UserRoles.Production_Manufacturing).
                                SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();
                        }
                        if (notifyQA_Sampling)
                        {
                             _notificationHub.Clients.Group(UserRoles.QA_Sampling).SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();
                              _notificationHub.Clients.Group(UserRoles.QA_Sampling).
                                SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();
                        }

                        if (notifyFilling_CheckEquipements)
                        {
                            _notificationHub.Clients.Group(UserRoles.Filling_CheckEquipements).SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();
                              _notificationHub.Clients.Group(UserRoles.Filling_CheckEquipements).SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();
                        }

                        if (notifyFilling_FillingTubes)
                        {
                             _notificationHub.Clients.Group(UserRoles.Filling_FillingTubes).SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();
                              _notificationHub.Clients.Group(UserRoles.Filling_FillingTubes).SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();

                        }
                        if (notifyFilling_Cartooning)
                        {
                             _notificationHub.Clients.Group(UserRoles.Filling_Cartooning).SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();
                              _notificationHub.Clients.Group(UserRoles.Filling_Cartooning).SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();
                        }

                        if (notifyFilling_Packaging)
                        {
                            _notificationHub.Clients.Group(UserRoles.Filling_Packaging).SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();
                              _notificationHub.Clients.Group(UserRoles.Filling_Packaging).SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();

                        }
                        if (notifyManager)
                        {
                              _notificationHub.Clients.Group(UserRoles.Manager).SendAsync("ReceiveMessage",newTaskMessage).GetAwaiter().GetResult();
                                  _notificationHub.Clients.Group(UserRoles.Manager).SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();
                        }
                        #endregion

                    }
                    catch(Exception)
                    {
                        transaction.Rollback();
                    }
              }
            

             
            

             return isCompleted;
        }

        public bool IsPreProductionTask(int _taskIdtypeId,int _departmentId)
        {
           bool ispreprodTask=false;

            if (_taskIdtypeId==(int)TaskTypesEnum.RawMaterialsWeighting)
            {
                if (_departmentId==(int) DepartmentsEnum.Warehouse||
                _departmentId==(int) DepartmentsEnum.QA||
                _departmentId==(int) DepartmentsEnum.Accounting)
                {
                    ispreprodTask=true;
                }
                
            }
            else if  (_taskIdtypeId==(int)TaskTypesEnum.RoomCleaning)
            {
                   if (_departmentId==(int) DepartmentsEnum.Warehouse)
                {
                    ispreprodTask=true;
                }  
            }
            else if  (_taskIdtypeId==(int)TaskTypesEnum.Equipments_Machines)
            {
                     if (_departmentId==(int) DepartmentsEnum.Production||
                _departmentId==(int) DepartmentsEnum.QA)
                {
                    ispreprodTask=true;
                }
            }
           return ispreprodTask;
        }

        public bool IsAllPreProductionTasksCompleted(DataContext dtcntxt, int _batchId,int _excludedTaksId)
     {
     bool iscompleted=false;
      

            List<BatchTask> batchTasks=new  List<BatchTask>();
            batchTasks = dtcntxt.BatchTasks.Where(x=>x.BatchId==_batchId).ToList();
            

        // one way to do it is to set is completed flag to true by default (not the best )
        // and then  check if any of the tasks is not completed set to false
        /*
         iscompleted=true; 
        foreach(var batchTaskitem in batchTasks)
        {
            if (IsPreProductionTask(batchTaskitem.TaskTypeId,(int)batchTaskitem.DepartmentId)&&batchTaskitem.Id!=_excludedTaksId)
            {
                if (batchTaskitem.TaskStateId!=(int)TaskStatesEnum.finished)
                {
                    iscompleted=false;
                    break;
                }
            }
        }
        */


        // other way ....
        int completedTasksCount=0; 
        int allTasksCount=0;
        foreach(var batchTaskitem in batchTasks)
        {
            if (IsPreProductionTask(batchTaskitem.TaskTypeId,(int)batchTaskitem.DepartmentId)&&batchTaskitem.Id!=_excludedTaksId)
            {
                allTasksCount++;
                if (batchTaskitem.TaskStateId==(int)TaskStatesEnum.finished)
                {
                   completedTasksCount++;
                }
            }
        }
        if (completedTasksCount==allTasksCount)
        {
             iscompleted=true;
        }


       
    
     return iscompleted;
     }

        public bool IsPreFillingTask( int _taskIdtypeId,int _departmentId)
        {
           bool isprefillingTask=false;

            if (_taskIdtypeId==(int)TaskTypesEnum.Sampling)
            {
                if (
                _departmentId==(int) DepartmentsEnum.QA)
                {
                    isprefillingTask=true;
                }
                
            }
            else if  (_taskIdtypeId==(int)TaskTypesEnum.Equipments_Machines)
            {
                   if (_departmentId==(int) DepartmentsEnum.Filling)
                {
                    isprefillingTask=true;
                }  
            }
             
           return isprefillingTask;
        }
     public bool IsAllPreFillingTasksCompleted(DataContext dtcntxt,int _batchId,int _excludedTaksId)
     {
     bool iscompleted=false;
       
        

            List<BatchTask> batchTasks=new  List<BatchTask>();
            batchTasks = dtcntxt.BatchTasks.Where(x=>x.BatchId==_batchId).ToList();
           

        int completedTasksCount=0; 
        int allTasksCount=0;
        foreach(var batchTaskitem in batchTasks)
        {
            if (IsPreFillingTask(batchTaskitem.TaskTypeId,(int)batchTaskitem.DepartmentId)&&batchTaskitem.Id!=_excludedTaksId)
            {
                allTasksCount++;
                if (batchTaskitem.TaskStateId==(int)TaskStatesEnum.finished)
                {
                   completedTasksCount++;
                }
            }
        }
        if (completedTasksCount==allTasksCount)
        {
             iscompleted=true;
        }


       
    
     return iscompleted;
     }
        
    }
}