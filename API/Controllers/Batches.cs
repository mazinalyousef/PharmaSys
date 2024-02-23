using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Data;
using API.Interfaces;
using API.BusinessLogic;
using API.Hubs;
using Microsoft.AspNetCore.SignalR;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Batches : ControllerBase
    {
        
         private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        
        private readonly IBatchRepository _batchRepository;
        private readonly IBatchBL _batchBL;

        private readonly IHubContext<NotificationHub> _notificationHub;

        public Batches(IBatchRepository batchRepository,
         IConfiguration configuration,
         IHubContext<NotificationHub> notificationHub,
         IBatchBL batchBL,
         IMapper mapper)
        {
            _batchBL = batchBL;
            _batchRepository = batchRepository;
            _notificationHub =notificationHub;
             _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BatchForViewDTO>>> GetBatches()
        {
                var Batches = await _batchRepository.GetAll();
                var batchesforview = _mapper.Map<IEnumerable< BatchForViewDTO>>(Batches);
               return Ok(batchesforview) ;
        }  


        [HttpGet("{Id}")]
        public async Task<ActionResult<BatchForEditDTO>> GetBatch(int Id)
        {
            var Batch  = await _batchRepository.Get(Id);
            var BatchforeditDTO = _mapper.Map<BatchForEditDTO>(Batch);
            if (Batch==null)
            { return NotFound();}
            else
            { return Ok(BatchforeditDTO);} 
        }
        

         [HttpPost]
        public async Task<IActionResult> Register([FromBody]BatchForEditDTO batchForEditDTO)
        {
                // mapping....
                  var batch=  _mapper.Map<Batch>(batchForEditDTO);
                  
                  /*
                   Batch batch=new Batch ()
                   {
                    ProductId = batchForEditDTO.ProductId,
                    BatchNO  =batchForEditDTO.BatchNO,
                    BatchSize=batchForEditDTO.BatchSize,
                    Barcode = batchForEditDTO.Barcode,
                    StartDate = batchForEditDTO.StartDate,
                    EndDate=batchForEditDTO.EndDate,
                    MFgDate=batchForEditDTO.MFgDate,
                    MFNO=batchForEditDTO.MFNO,
                    UserId = batchForEditDTO.UserId,
                    TubePictureURL=batchForEditDTO.TubePictureURL,
                    CartoonPictureURL= batchForEditDTO.CartoonPictureURL,
                    TubeWeight =batchForEditDTO.TubeWeight,
                    TubesCount =batchForEditDTO.TubesCount,
                    BatchStateId = batchForEditDTO.BatchStateId
                   };
                   */

              //  var result = await _batchRepository.Add(batch);
                  var result = await _batchRepository.Add_Batch_Dataset(batch);
                 // make sure about  the checking here....
                  if (result>0)
                  {
                      return Ok(new Response { Status = "Success", Message = "Batch Register Was Successfull!" });
                  }
                  else
                  {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed To Create the Batch" });
                  }
           
        }

    
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update ([FromBody]BatchForEditDTO batchForEditDTO,int Id)
        {
              var batch=  _mapper.Map<Batch>(batchForEditDTO);
              var result = await _batchRepository.Update(Id,batch); // catch exceptions ... later....
              if (result==null)
           {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Batch Update Failed!"});            
           }

              return Ok(new Response { Status = "Success", Message = "Batch Update Was Successfull!" });
        }



        [HttpDelete("{Id}")]

        public ActionResult Delete (int Id)
        {
            // find another (better) way than try catch
            try 
            {
              _batchRepository.Delete(Id);
              return NoContent();
            }
            catch(Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message =ex.Message});           
            }
            
        }




        [HttpPost]
        [Route("send/{Id}")]
        public async Task<IActionResult> SendBatch( int Id)
        {
             // note we can return an object (batch DTO) instead of bool to get  more info ...
              bool completed = await _batchBL.SendBatch(Id);
              string newTaskMessage="You've Received a New Task";


               if (completed)
                {
                     // send notifications to the associated groups...
               await _notificationHub.Clients.Group(UserRoles.Warehouse_CheckRooms).SendAsync("ReceiveMessage",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.Warehouse_RawMaterials).SendAsync("ReceiveMessage",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.QA_RawMaterials).SendAsync("ReceiveMessage",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.QA_CheckEquipements).SendAsync("ReceiveMessage",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.Production_CheckEquipements).SendAsync("ReceiveMessage",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.Accountant).SendAsync("ReceiveMessage",newTaskMessage);

                // will also send notifications to clients so that they must update the notifications list
               await _notificationHub.Clients.Group(UserRoles.Warehouse_CheckRooms).SendAsync("UpdateNotifications",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.Warehouse_RawMaterials).SendAsync("UpdateNotifications",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.QA_RawMaterials).SendAsync("UpdateNotifications",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.QA_CheckEquipements).SendAsync("UpdateNotifications",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.Production_CheckEquipements).SendAsync("UpdateNotifications",newTaskMessage);
               await _notificationHub.Clients.Group(UserRoles.Accountant).SendAsync("UpdateNotifications",newTaskMessage);
                } 
             
                   
             return Ok(completed);
        }

    }
}