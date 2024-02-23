using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.DTOS;
using API.Entities;
using API.Hubs;
using API.Interfaces;
using API.Timers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Tasks : ControllerBase
    {
          private readonly IHubContext<TaskTimerHub> _tasktimerhub;

            private readonly IHubContext<NotificationHub> _notificationHub;
          private readonly TaskTimer _tasktimer;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        private readonly ITaskBL _taskBL;

        private readonly UserManager<User> _userManager;
        public Tasks(ITaskRepository taskRepository,IMapper mapper,ITaskBL taskBL,
        IHubContext<TaskTimerHub> tasktimerhub,TaskTimer taskTimer, 
        IHubContext<NotificationHub> notificationHub
        ,UserManager<User> userManager)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _taskBL= taskBL;
             _tasktimerhub = tasktimerhub;
                _tasktimer=taskTimer;
                _notificationHub=notificationHub;
                _userManager=userManager;
            
        }


        [HttpPut]
        [Route("Assign")]
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
                if ( taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.RoomCleaning)
                {  _tasktimer.MaxSeconds=50;}
                else  if ( taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.RawMaterialsWeighting)
                {
                _tasktimer.MaxSeconds=500;
                }
                 int secondsleft=-1;
             
                if (!_tasktimer.IsTimerStarted)
                _tasktimer.PrepareTimer
                (
                () =>
                {
                 
                      _tasktimerhub.Clients.Group(taskAssignDTO.TaskId.ToString()).SendAsync("TransferTimerData",
                      _tasktimer.currentSeconds);
                        secondsleft=_tasktimer.currentSeconds;
                } 
                );
            return Ok(true);

        }


        [HttpGet("{Id}")]
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
        public  ActionResult<bool> SetTaskAsCompleted(int Id)
        {
            
           bool iscompleted=_taskBL.SetAsCompleted(Id);
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
        public async Task<ActionResult<IEnumerable<UserRunningTaskDTO>>> GetUserRunningTasks(string Id)
        {
            var tasks  = await _taskRepository.GetUserRunningTasks(Id);
            var tasksforview = _mapper.Map<IEnumerable< UserRunningTaskDTO>>(tasks);
            return Ok(tasksforview) ;
         
        }








        
    }
}