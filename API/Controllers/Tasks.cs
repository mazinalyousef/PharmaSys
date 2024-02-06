using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Tasks : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public Tasks(ITaskRepository taskRepository,IMapper mapper)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            
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





        
    }
}