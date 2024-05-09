using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
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
        private readonly UserManager<User> _userManager;
        private readonly DataContext _dataContext;
        
     public BatchBL( 
     UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        DataContext dataContext)
     {
 
             _userManager =userManager;
             _dataContext=dataContext;
             
     }

     
     public async Task<bool>  SendBatch(int _batchId)
     {

         bool Sendcompleted=false;
        // wrap in one transaction later...
         // for now use transaction here ---
         // -- in future implement unit of work design pattern

        // using (IDbContextTransaction)

        // first generate the tasks for the whole system 
        // Note : this could be done in a loop for pre-defined list --
        // -- but the department Id is the difference key here...--
        // -- and the same task can be delivered to more than one department 
        // 
         
          var allusers= await _userManager.Users.ToListAsync();
          var Warehouse_CheckRoomsusers=  await _userManager.GetUsersInRoleAsync(UserRoles.Warehouse_CheckRooms);
         // var  Warehouse_RawMaterialsusers=  await _userManager.GetUsersInRoleAsync(UserRoles.Warehouse_RawMaterials);
         //  var  QA_RawMaterialsusers=  await _userManager.GetUsersInRoleAsync(UserRoles.QA_RawMaterials);
          var  QA_CheckEquipementsusers=  await _userManager.GetUsersInRoleAsync(UserRoles.QA_CheckEquipements);
          var  Production_CheckEquipementsusers=  await _userManager.GetUsersInRoleAsync(UserRoles.Production_CheckEquipements);
          var  Accountantusers=  await _userManager.GetUsersInRoleAsync(UserRoles.Accountant);

         using (IDbContextTransaction transaction=await _dataContext.Database.BeginTransactionAsync())
         {
        try 
         {
         #region Pre-Production Tasks

        //     cleaning room   
         var batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Warehouse;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RoomCleaning;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
           var result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
            int RoomCleaningTaskId= result.Entity.Id;
 
          

         //    Raw materials Weighting
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Warehouse;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RawMaterialsWeighting;
         batchtask.DurationInSeconds = -1; // alter later....
        // batchtask.StartDate = DateTime.Now;
            result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
          int RawMaterialsWeightingTaskId=  result.Entity.Id;

         // Raw materials check -- QA
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RawMaterialsWeighting;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
           result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
         int RawMaterialsCheckQATaskId =  result.Entity.Id;


          // equipement check -- QA
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
        // batchtask.StartDate = DateTime.Now;
           result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
           int Equipments_MachinesCheckQATaskId=  result.Entity.Id;


          // equipement --  production 
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Production;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
          result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
         int Equipments_MachinesCheckProductionTaskId=   result.Entity.Id;


        // raw materials -- Accountant   
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Accounting;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.RawMaterialsWeighting;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
          result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
         int RawMaterialsCheckAccountantTaskId=  result.Entity.Id;


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
         _dataContext.BatchTasks.Add(batchtask);
         await _dataContext.SaveChangesAsync();
      


          // check enviroment -- production
          batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Production;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Enviroment;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
          result = _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
          int EnviromentTaskId= result.Entity.Id;


           // sampling -- qa
          batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Sampling;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
             _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();
        



         // equipements -- filling
          batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now;
         
          _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();


         // equipements -- QA (Again??)

         /*
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.QA;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Equipments_Machines;
         batchtask.DurationInSeconds = -1; // alter later....
     
           await   _taskRepository.Add(batchtask);
           */




        // filling tubes -- filling
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.FillingTubes;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
        _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();


          // cartooning  -- filling
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Cartooning;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
      _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();




          // packaging   -- filling
         batchtask = new BatchTask();
         batchtask.BatchId  = _batchId;
         batchtask.DepartmentId = (int) DepartmentsEnum.Filling;
         batchtask.TaskStateId = (int)  TaskStatesEnum.initialized;
         batchtask.TaskTypeId = (int)TaskTypesEnum.Packaging;
         batchtask.DurationInSeconds = -1; // alter later....
       //  batchtask.StartDate = DateTime.Now; 
         _dataContext.BatchTasks.Add(batchtask);
            await _dataContext.SaveChangesAsync();


        #endregion 

      
         // update batch info
          var originalBatch=await _dataContext.Batches
          .FirstOrDefaultAsync(x=>x.Id==_batchId);


          //
           // set as completed...
            originalBatch.StartDate = DateTime.Now;
            originalBatch.BatchStateId =(int) BatchStatesEnum.preProduction;
           _dataContext.Entry(originalBatch).Property(x=>x.StartDate).IsModified=true;
            _dataContext.Entry(originalBatch).Property(x=>x.BatchStateId).IsModified=true;

            await _dataContext.SaveChangesAsync();
             
         
        // add notifications....
      
         #region  global notification
         // add (for every user ) global notification that batch has started...
       
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
             _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
           
         }


         #endregion

         // add notifications to the associated roles .....
         #region Notifications To Associated Users
         // add cleaning room notifications...
       
        foreach (var userItem in Warehouse_CheckRoomsusers)
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
            _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
         }


           // add Weighting raw materials task notifications...
        /*
        foreach (var userItem in Warehouse_RawMaterialsusers)
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
            _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
         }
         */


        // add Raw Materials Check QA Task notifications...
       /*
        foreach (var userItem in QA_RawMaterialsusers)
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
            _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
         }

         */


         // add Equipments _Machines Check QA Task  notifications...
       
        foreach (var userItem in QA_CheckEquipementsusers)
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
           _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
         }


         // add Equipments _Machines Check production Task  notifications...
       
        foreach (var userItem in Production_CheckEquipementsusers)
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
            _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
         }


        // add Raw Materials Check Accountant Task notifications...
        
        foreach (var userItem in Accountantusers)
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
            _dataContext.Notifications.Add(notification);
             await _dataContext.SaveChangesAsync();
         }



          #endregion
      
               await transaction.CommitAsync();
               Sendcompleted =true;    
               }
               catch(Exception)
               {
                 await transaction.RollbackAsync();
               }
             
         }
     

       

         return Sendcompleted;
        
     }

    }
}