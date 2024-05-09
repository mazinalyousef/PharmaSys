using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Helpers;
using API.Hubs;
using API.Interfaces;
using API.Timers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Tasks : ControllerBase
    {
          private readonly IHubContext<TaskTimerHub> _tasktimerhub;
             private readonly IHubContext<TaskReminderHub> _taskreminderhub;

            private readonly IHubContext<NotificationHub> _notificationHub;
          private readonly TaskTimer _tasktimer;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        private readonly ITaskBL _taskBL;

        private readonly UserManager<User> _userManager;

         private readonly DataContext _dataContext;


         private   readonly object completeTaskLock=new object ();

        public Tasks(ITaskRepository taskRepository,IMapper mapper,ITaskBL taskBL,
        IHubContext<TaskTimerHub> tasktimerhub,TaskTimer taskTimer, 
        IHubContext<TaskReminderHub> taskreminderhub,
        IHubContext<NotificationHub> notificationHub
        ,UserManager<User> userManager,DataContext dataContext)
        { _dataContext=dataContext;
            _mapper = mapper;
            _taskRepository = taskRepository;
            _taskBL= taskBL;
             _tasktimerhub = tasktimerhub;
             _taskreminderhub = taskreminderhub;
                _tasktimer=taskTimer;
                _notificationHub=notificationHub;
                _userManager=userManager;
            
        }



        [HttpPut]
        [Route("StartReminder")]
        [Authorize]
        public async Task<ActionResult<bool>>StartReminder([FromBody]TaskAssignDTO taskAssignDTO)
        {
          bool iscompleted=false;

          // get the task timer ....
             var taskinfo=await _taskRepository.getBatchTaskInfo(taskAssignDTO.TaskId);

             
               // try to get the suitable timer ....
              var timerItem= Helpers.TaskTypesTimerData.taskTypesTimers.Where(x=>x.DepartmentId==taskinfo.DepartmentId&&x.taskTypeId
              ==taskinfo.TaskTypeId).FirstOrDefault();
                
              if (timerItem!=null)
              {
                  _tasktimer.MaxSeconds = timerItem.DurationInSeconds;
              }
                 string message="";
              if (taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.FillingTubes)
              { 
                    message="Filling Tubes Reminder : Check Machines.....";
                      _tasktimer.TimerTickPeriod=300000; // 
              }
              else  if (taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.Cartooning)
              {
                    message="Cartooning Reminder : Check Machines.....";
                      _tasktimer.TimerTickPeriod=300000; // 
              }

              int secondsleft=-1;
            

               if (!_tasktimer.IsTimerStarted)
                _tasktimer.PrepareTimer
                (
                async () =>
                {
                  await _taskreminderhub.Clients.Group(taskAssignDTO.TaskId.ToString()).SendAsync("TransferReminderData",
                  message);
                  secondsleft=_tasktimer.currentSeconds;   
                } 
                );


          return Ok(iscompleted);
        }



        [HttpPut]
        [Route("Assign")]
        [Authorize]
        public async Task<ActionResult<bool>> SetTaskAsAssigned([FromBody] TaskAssignDTO taskAssignDTO)
        {
           bool iscompleted=  await _taskRepository.
           SetAsAssigned(taskAssignDTO.TaskId,taskAssignDTO.UserId);

           if (!iscompleted)
           {
                /*
            return StatusCode(StatusCodes.Status500InternalServerError,new Response()
            {
                    Status="Error",Message="Requset Failed To Complete"
            });
            */
             return StatusCode(StatusCodes.Status500InternalServerError,false);
           }
           
               // make user update their notifications list...

                // we can make filtering based on the tasktype Id to who we will send the notifications
                
                // get the group name that we want to send notifications to --
                // -- so that the group will update the notifications list
                // get the group name from user ID


                var userEntity = await _userManager.FindByIdAsync(taskAssignDTO.UserId);
                var userroles   = await _userManager.GetRolesAsync(userEntity);
                foreach(string role in userroles)
                {
                  await _notificationHub.Clients.Group(role).SendAsync("UpdateNotifications","Update Notifications");
                }
                 // fire the timer ....
                var taskinfo= _taskRepository.getBatchTaskInfo(taskAssignDTO.TaskId).GetAwaiter().GetResult();

                // we need to include the department also ....



                // try to get the suitable timer ....
                var timerItem= Helpers.TaskTypesTimerData.taskTypesTimers.Where(x=>x.DepartmentId==taskinfo.DepartmentId&&x.taskTypeId
                 ==taskinfo.TaskTypeId).FirstOrDefault();

                 if (timerItem!=null)
                 {
                     _tasktimer.MaxSeconds = timerItem.DurationInSeconds;
                 }




                /*
                if ( taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.RoomCleaning)
                { 
                   _tasktimer.MaxSeconds=15;
                }
                else  if ( taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.RawMaterialsWeighting)
                {
                _tasktimer.MaxSeconds=25;
                }
                else  if ( taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.Manufacturing)
                {
                _tasktimer.MaxSeconds=22;
                }
                */
                 int secondsleft=-1;


                 // when task is assigned the timer started ...
                 // however the timer will continue ticking down till reach the zero seconds (till reach its minimum value) --
                 // -- neglecting the fact that the task may be ended ...
                 // -- in other words : completing the task will never stop the timer (for now) --
                 // -- it will just make the user leave the task group.....


                 // 
                 _tasktimer.TimerTickPeriod=1000;

                if (!_tasktimer.IsTimerStarted)
                _tasktimer.PrepareTimer
                (
                async () =>
                {
                 

                       
                       await _tasktimerhub.Clients.Group(taskAssignDTO.TaskId.ToString()).SendAsync("TransferTimerData",
                      _tasktimer.currentSeconds);
                       secondsleft=_tasktimer.currentSeconds;   
                } 
                );

                         

                        // execute code after the timer seconds.....

                       
                       //  Task.Delay(_tasktimer.MaxSeconds*1000).ContinueWith(o => { OnTimerrElapsed(taskAssignDTO.TaskId); });     
                         return Ok(true);

                   }




                  [HttpPost]
                  [Route("WaitForTaskTimer")]
                  [Authorize]
                   public async Task<ActionResult<bool>> WaitForTaskTimer([FromBody] TaskAssignDTO taskAssignDTO)
                   {

                          
                         // int originalSeconds=taskAssignDTO.Seconds; //  coming from client ....not the best .. change later...

                            int originalSeconds=-1;
                           var timerItem= Helpers.TaskTypesTimerData.taskTypesTimers.Where(x=>x.DepartmentId==taskAssignDTO.DepartmentId&&x.taskTypeId
                           ==taskAssignDTO.TaskTypeId).FirstOrDefault();
                            if (timerItem!=null)
                             {
                              originalSeconds = timerItem.DurationInSeconds;
                             }
                           
                           if (originalSeconds>0)
                           {


                            await Task.Delay((originalSeconds*1000)+5000).ContinueWith(o =>
                           {
                        
                                      try
                                     {
                                     // send notification to manager that task timer is ended.....

                                       
                                      // get managers
                                    
                                    var ManagerUsers= _userManager.GetUsersInRoleAsync(UserRoles.Manager).GetAwaiter().GetResult();;
                                    var  originalTask=  _dataContext.BatchTasks.
                                    Include(x=>x.Batch).ThenInclude(x=>x.Product)
                                   .Include(x=>x.TaskType).Where(x=>x.Id==taskAssignDTO.TaskId).FirstOrDefaultAsync().GetAwaiter().GetResult();;
                                    var originalUser = _userManager.FindByIdAsync(originalTask.UserId).GetAwaiter().GetResult();;
                                    
                                    if (originalTask.TaskStateId!=(int)Enumerations.TaskStatesEnum.finished)
                                    {
                                    string message = string.Format("Timer Elapsed For Task : {0} ,Batch NO: {1} , Assigned By User :{2} ",
                                     originalTask.TaskType.Title,originalTask.Batch.BatchNO,originalUser.UserName
                                     );

                                      // for all managers send  notofications DB 

                                    bool tscommited=false;
                                     using (IDbContextTransaction transaction=  _dataContext.Database.BeginTransactionAsync().GetAwaiter().GetResult())
                                    {
                                      try 
                                      {
                                        foreach (var managerUser in ManagerUsers)
                                      {
                                        var userId = managerUser.Id;
                                        Entities.Notification notification=new  Entities.Notification();
                                        notification.BatchId = originalTask.BatchId;
                                        notification.BatchTaskId =null;
                                        notification.DateSent = DateTime.Now;

                                        // need to get the batch number 
                                        notification.NotificationMessage = message;
                                        notification.UserId = userId;
                                       
                                        _dataContext.Notifications.Add(notification);
                                          _dataContext.SaveChangesAsync().GetAwaiter().GetResult(); 
                                        }

                                          transaction.CommitAsync().GetAwaiter().GetResult();;tscommited=true;
                                      }
                                      catch(Exception ex)
                                      {
                                        var x=ex.ToString();
                                            transaction.RollbackAsync().GetAwaiter().GetResult();;
                                      }
                                  
                                   }  // using transaction...

                                  // send notification messages .. hub...
                                    if (tscommited)
                                    {
                                        _notificationHub.Clients.Group(UserRoles.Manager).SendAsync("ReceiveMessage",message).GetAwaiter().GetResult();;
                                        _notificationHub.Clients.Group(UserRoles.Manager).SendAsync("UpdateNotifications","UpdateNotifications").GetAwaiter().GetResult();;
                                    }

                                   } // if task not finished...
                                     }
                                     catch(Exception ex)
                                     {
                                        var xxx=ex.ToString();
                                     }
                        
                         
                         
                       }
                       );


                           }
                    

                         return Ok(true);   

                   }






        [HttpGet("{Id}")]
          [Authorize]
        public async Task<ActionResult<BatchTaskInfoDTO>> getBatchTaskinfo(int Id)
        {
            var originalItem=  await  _taskRepository.getBatchTaskInfo(Id);
            if (originalItem!=null)
            {
                BatchTaskInfoDTO batchTaskInfoDTO =_mapper.Map<BatchTaskInfoDTO>(originalItem);
                 return Ok(batchTaskInfoDTO); 
            }
           return StatusCode(StatusCodes.Status500InternalServerError,new Response()
            {
                    Status="Error",Message="Original Item Not Exists.."
            });
        }


        [HttpGet]
        [Route("checkedList/{Id}")]
         [Authorize]
        public async Task<ActionResult<CheckedListTaskForViewDTO>> getCheckedListBatchTask(int Id)
        {
            var item = await _taskRepository.getCheckedListTaskForView(Id);
            if (item!=null)
            {
                 return Ok(item); 
            }
            return StatusCode(StatusCodes.Status500InternalServerError,new Response()
            {
                    Status="Error",Message="Original Item Not Exists.."
            });
        }

        [HttpGet]
        [Route("rawMaterials/{Id}")]
          [Authorize]
        public async Task<ActionResult<RawMaterialsTaskForViewDTO>> getrawMaterialsBatchTask(int Id)
        {
            var item = await _taskRepository.GetRawMaterialsTaskForView(Id);
            if (item!=null)
            {
                 return Ok(item); 
            }
            return StatusCode(StatusCodes.Status500InternalServerError,new Response()
            {
                    Status="Error",Message="Original Item Not Exists.."
            });
        }


        [HttpGet]
        [Route("rangeSelect/{Id}")]
          [Authorize]
        public async Task<ActionResult<RangeSelectTaskForViewDTO>> getRangeSelectTaskBatchTask(int Id)
        {
            var item = await _taskRepository.GetRangeSelectTaskForViewDTO(Id);
            if (item!=null)
            {
                 return Ok(item); 
            }
            return StatusCode(StatusCodes.Status500InternalServerError,new Response()
            {
                    Status="Error",Message="Original Item Not Exists.."
            });
        }

        [HttpPost]
        [Route("complete/{Id}")]
          [Authorize]
        public  ActionResult<bool> SetTaskAsCompleted(int Id)
        {
           bool iscompleted=false;
           lock(completeTaskLock)
           {
            iscompleted=_taskBL.SetAsCompleted(Id);
           }
         
           if (!iscompleted)
           {
                /*
            return StatusCode(StatusCodes.Status500InternalServerError,new Response()
            {
                    Status="Error",Message="Requset Failed To Complete"
            });
            */
             return StatusCode(StatusCodes.Status500InternalServerError,false);
           }

            return Ok(true);

        }

        [HttpGet]
        [Route("userTasks/{Id}")]
          [Authorize]
        public async Task<ActionResult<IEnumerable<UserRunningTaskDTO>>> GetUserRunningTasks(string Id)
        {
            var tasks  = await _taskRepository.GetUserRunningTasks(Id);
            var tasksforview = _mapper.Map<IEnumerable< UserRunningTaskDTO>>(tasks);
            return Ok(tasksforview) ;
         
        }

        [HttpGet]
        [Route("BtachTasksSummary/{Id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BatchTaskSummaryDTO>>> GetBatchTaskSummaries(int Id)
        {
            var result= await _taskRepository.GetBatchTaskSummaries(Id);
            return Ok(result);
        }




      





        
    }
}