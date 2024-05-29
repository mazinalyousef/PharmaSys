using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Hubs;
using API.Interfaces;
using API.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTimerController : ControllerBase
    {
         private readonly IHubContext<TaskTimerHub> _tasktimerhub;
          private readonly TaskTimer _taskTimer;
           private readonly ITaskRepository _taskRepository;

      public TaskTimerController(IHubContext<TaskTimerHub> tasktimerhub,
      TaskTimer taskTimer,
      ITaskRepository taskRepository
      )
     {
        _tasktimerhub = tasktimerhub;
         
        _taskTimer=taskTimer;

        _taskRepository = taskRepository;
         

     }

         [HttpGet("{Id}")]
         public ActionResult<int> Get(int Id)
       {
                 int secondsleft=-1;
                
                
                // used in task assign controller Instead...

                /*
                var taskinfo= _taskRepository.getBatchTaskInfo(Id).GetAwaiter().GetResult();
                if ( taskinfo.TaskTypeId==(int)Enumerations.TaskTypesEnum.RoomCleaning)
                {
                if (!_taskTimer.IsTimerStarted)
                {
                _taskTimer.PrepareTimer
                (
                () =>
                {
               
                  _tasktimerhub.Clients.Group(Id.ToString()).SendAsync("TransferTimerData",_taskTimer.currentSeconds);
                  secondsleft=_taskTimer.currentSeconds;
                }
                );
                }
                } 
                */
                 
                
              
               return Ok(secondsleft);
    }
    }
}