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
    public class BatchRecords : ControllerBase
    {
         private readonly IWebHostEnvironment _hostingEnvironment;
        
         private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        
        private readonly IBatchRepository _batchRepository;
         
         private readonly ITaskRepository _taskRepository;

       

        public BatchRecords(IBatchRepository batchRepository,ITaskRepository taskRepository,
         IConfiguration configuration,
        
         IMapper mapper,IWebHostEnvironment hostingEnvironment)
        {
            
           
            _batchRepository = batchRepository;
           _taskRepository=taskRepository;
             _mapper = mapper;
            _configuration = configuration;
              _hostingEnvironment  = hostingEnvironment;
        }

           [HttpGet("{Id}")]
       // [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<BatchManufacturingRecordDTO>> GetBatchManufacturingRecord(int Id)
        {
            BatchManufacturingRecordDTO batchManufacturingRecordDTO =new  BatchManufacturingRecordDTO();
            var Batch  = await _batchRepository.Get(Id);
            var BatchforeditDTO = _mapper.Map<BatchForEditDTO>(Batch);
            if (Batch==null)
            { return NotFound();}
            batchManufacturingRecordDTO.batchForEditDTO=BatchforeditDTO;

            // get all the related tasks... 
            var tasksummaries= await _taskRepository.GetBatchTaskSummaries(Id);
            
            foreach (var item in tasksummaries)
            {
                // check the Base type whether it is a checked list or raw material ....
                 
                if (item.TaskTypeId==(int)Enumerations.TaskTypesEnum.RawMaterialsWeighting)
                {
                  var taskItem= await _taskRepository.GetRawMaterialsTaskForView(item.Id);
                  batchManufacturingRecordDTO.rawMaterialsTaskForViewDTOs.Add(taskItem);
                }
                else 
                {
                     var taskItem= await _taskRepository.getCheckedListTaskForView(item.Id);
                     batchManufacturingRecordDTO.checkedListTaskForViewDTOs.Add(taskItem);
                }
             }


            return Ok(batchManufacturingRecordDTO);
        }
                
         
    }
}