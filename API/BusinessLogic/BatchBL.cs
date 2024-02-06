using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Enumerations;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.BusinessLogic
{
    public class BatchBL : IBatchBL
    {

       private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
       private readonly IBatchRepository _batchRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly INotificationRepository _notificationRepository;
        
     public BatchBL(IBatchRepository batchRepository,
     ITaskRepository taskRepository,
     INotificationRepository notificationRepository,
     UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
     {
            _notificationRepository = notificationRepository;
            _taskRepository = taskRepository;
            _batchRepository = batchRepository;
             _userManager =userManager;
             _roleManager = roleManager;
     }

     
     public async Task<bool>  SendBatch(int _batchId)
     {

        bool Sendcompleted=false;
        // wrap in one transaction later...

        // using (IDbContextTransaction)

        // first generate the tasks for the whole system 
        // Note : this could be done in a loop for pre-defined list --
        // -- but the department Id is the difference key here...--
        // -- and the same task can be delivered to more than one department 
        // 

        #region Pre-Production Tasks

        //     cleaning room   
         var batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Warehouse;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RoomCleaning;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
         int RoomCleaningTaskId=  await _taskRepository.Add(batchtask);
 

         //    Raw materials Weighting
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Warehouse;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RawMaterialsWeighting;
         batchtask.DurationInSeconds = -1; // alter later....
        // batchtask.StartDate = DateTime.Now;
         
          int RawMaterialsWeightingTaskId=    await  _taskRepository.Add(batchtask);


         // Raw materials check -- QA
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RawMaterialsWeighting;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
         int RawMaterialsCheckQATaskId =  await  _taskRepository.Add(batchtask);


          // equipement check -- QA
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
        // batchtask.StartDate = DateTime.Now;
         
        int Equipments_MachinesCheckQATaskId=  await  _taskRepository.Add(batchtask);


          // equipement --  production 
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Production;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
         int Equipments_MachinesCheckProductionTaskId=   await  _taskRepository.Add(batchtask);


        // raw materials -- Accountant   
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Accounting;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RawMaterialsWeighting;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
         int RawMaterialsCheckAccountantTaskId= await _taskRepository.Add(batchtask);


          #endregion


        #region Production Tasks

        //    manufacturing -- production
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Production;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Manufacturing;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
       await  _taskRepository.Add(batchtask);


          // check enviroment -- production
          batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Production;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Enviroment;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
        int EnviromentTaskId= await _taskRepository.Add(batchtask);


           // sampling -- qa
          batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Sampling;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
       await  _taskRepository.Add(batchtask);



         // equipements -- filling
          batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
       await  _taskRepository.Add(batchtask);



         // equipements -- QA (Again??)
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
      await   _taskRepository.Add(batchtask);




        // filling tubes -- filling
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.FillingTubes;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
       await  _taskRepository.Add(batchtask);


          // cartooning  -- filling
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Cartooning;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
      await   _taskRepository.Add(batchtask);




          // packaging   -- filling
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Packaging;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
         await   _taskRepository.Add(batchtask);


        #endregion 

      
         // update batch info
         var originalBatch =await _batchRepository.GetBatch(_batchId);
         var completed=  await _batchRepository.SetBatchAsStarted(_batchId);

        // add notifications....
      
         #region  global notification
         // add (for every user ) global notification that batch has started...
         var allusers= await _userManager.Users.ToListAsync();
         foreach (var userItem in allusers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} Has Started",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }


         #endregion

         // add notifications to the associated roles .....
         #region Notifications To Associated Users
         // add cleaning room notifications...
        var AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Warehouse_CheckRooms);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =RoomCleaningTaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With Room Cleaning Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }


           // add Weighting raw materials task notifications...
        AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Warehouse_RawMaterials);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =RawMaterialsWeightingTaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With Weighting raw materials Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }


        // add Raw Materials Check QA Task notifications...
        AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.QA_RawMaterials);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =RawMaterialsCheckQATaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With Raw Materials Check Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }


         // add Equipments _Machines Check QA Task  notifications...
        AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.QA_CheckEquipements);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =Equipments_MachinesCheckQATaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With Equipments & Machines Check  Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }


         // add Equipments _Machines Check production Task  notifications...
        AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Production_CheckEquipements);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =Equipments_MachinesCheckProductionTaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With Equipments & Machines Check  Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }


        // add Raw Materials Check Accountant Task notifications...
        AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Accountant);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =RawMaterialsCheckAccountantTaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With Raw Materials Check Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }




         // testing purposes .. remove later

           // enviroment
        AssociatedUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Production_Manufacturing);
        foreach (var userItem in AssociatedUsers)
         {
            var userId = userItem.Id;
            Notification notification = new Notification ();
            notification.BatchId = _batchId;
            notification.BatchTaskId =EnviromentTaskId;
            notification.DateSent = DateTime.Now;
            notification.NotificationMessage = string.Format("Batch {0} With enviroment  Task Is Available",
            originalBatch.BatchNO.ToString()
            );
            notification.UserId = userId;
            await _notificationRepository.Add(notification);
         }




          #endregion

         Sendcompleted =true;

         return Sendcompleted;
        
     }

    }
}