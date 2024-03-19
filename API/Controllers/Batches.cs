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
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Batches : ControllerBase
    {


        private readonly IWebHostEnvironment _hostingEnvironment;
        
         private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        
        private readonly IBatchRepository _batchRepository;
        private readonly IBatchBL _batchBL;

        private readonly IHubContext<NotificationHub> _notificationHub;

        public Batches(IBatchRepository batchRepository,
         IConfiguration configuration,
         IHubContext<NotificationHub> notificationHub,
         IBatchBL batchBL,
         IMapper mapper,IWebHostEnvironment hostingEnvironment)
        {
            _batchBL = batchBL;
            _batchRepository = batchRepository;
            _notificationHub =notificationHub;
             _mapper = mapper;
            _configuration = configuration;
              _hostingEnvironment  = hostingEnvironment;
        }

        [HttpGet]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<IEnumerable<BatchForViewDTO>>> GetBatches()
        {
                var Batches = await _batchRepository.GetAll();
                var batchesforview = _mapper.Map<IEnumerable< BatchForViewDTO>>(Batches);
               return Ok(batchesforview) ;
               
        }  

        [HttpGet("{Id}")]
        [Authorize(Policy ="ManagerPolicy")]
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
           [Authorize(Policy ="ManagerPolicy")]
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
                      return Ok(result);
                  }
                  else
                  {
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { Status = "Error", Message = "Failed To Create the Batch" });
                  }
           
        }

    
        [HttpPut("{Id}")]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> Update ([FromBody]BatchForEditDTO batchForEditDTO,int Id)
        {
              var batch=  _mapper.Map<Batch>(batchForEditDTO);

                  /*
                 if (batch.BatchStateId!=(int)Enumerations.BatchStatesEnum.initialized)
                {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Batch Update Failed Due To Batch State!"});     
                }
                */

              var result = await _batchRepository.Update(Id,batch); // catch exceptions ... later....
              if (result==null)
                {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Batch Update Failed!"});            
                }

              return Ok(new Response { Status = "Success", Message = "Batch Update Was Successfull!" });
        }



       

        [HttpDelete("{Id}")]
  [Authorize(Policy ="ManagerPolicy")]
        public ActionResult Delete (int Id)
        {
            // find another (better) way than try catch
                try 
                {

                  var originalEntity  = _batchRepository.Get(Id).GetAwaiter().GetResult();
                   if (originalEntity.BatchStateId==(int)Enumerations.BatchStatesEnum.initialized)
                   {
                   _batchRepository.Delete(Id);
                     return NoContent();
                   }
                   else
                   {
              return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Batch Delete Failed!"});    
                   }
             
            }
            catch(Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message =ex.Message});           
            }
            
        }




        [HttpPost]
        [Route("send/{Id}")]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> SendBatch( int Id)
        {
                bool completed =false;
                   var originalEntity  = _batchRepository.Get(Id).GetAwaiter().GetResult();
                   if (originalEntity.BatchStateId==(int)Enumerations.BatchStatesEnum.initialized)
                   {
                    // note we can return an object (batch DTO) instead of bool to get  more info ...
                completed = await _batchBL.SendBatch(Id);
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
             
                   }
                   else
                   {
                   return StatusCode(StatusCodes.Status500InternalServerError,
                     new Response { Status = "Error", Message = "Batch Send Failed!"});    
                   }

           
                   
             return Ok(completed);
        }


        
           [HttpPost]
           [Route("DeletePhotos")]
            public ActionResult <string> DeletePhotos(int Id)
            {
                  string err=string.Empty;
                  try 
                  {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string uploadsDir = Path.Combine(webRootPath, "uploads");
                    string fileName = Id.ToString()+"_Tube.jpg";
                    string fullPath = Path.Combine(uploadsDir, fileName);

                      if (System.IO.File.Exists(fullPath))
                      {
                        System.IO.File.Delete(fullPath);
                      }
                      fileName = Id.ToString()+"_Cartoon.jpg";
                      fullPath = Path.Combine(uploadsDir, fileName);
                     if (System.IO.File.Exists(fullPath))
                      {
                        System.IO.File.Delete(fullPath);
                      }
                    
                     
                  }
                  catch(Exception ex)
                  {
                      err =ex.Message;
                      
                  }
                 if (err.Length==0)
                  {
                    return Ok(err) ;
                   // return Ok(new Response { Status = "Success", Message = " delete Was Successfull!" });
                  }
                  else
                  {

                   //  return false;
                   return StatusCode(StatusCodes.Status500InternalServerError,
                       err);   
                  }
                  
            }


            		[HttpPost]
          [Route("UploadCartoonPhoto")]
          [DisableRequestSizeLimit]
           public async Task <IActionResult> AddCartoonPhoto()
          {
            // write photos to disk 
             string error=string.Empty;
              if (Request.Form.Files.Any())
              {
                 
                  try 
                  {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string uploadsDir = Path.Combine(webRootPath, "uploads");
                     // wwwroot/uploads/
                    if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                     // tube picture 
                     IFormFile file = Request.Form.Files[0];
                      var batchId= file.Name;
                   //  string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).file.Trim('"');
                     string fileName = batchId+"_Cartoon.jpg";
                      string fullPath = Path.Combine(uploadsDir, fileName);
                         var buffer = 1024 * 1024;
                      using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer, useAsync: false);
                      await file.CopyToAsync(stream);
                     await stream.FlushAsync();    
                  }
                  catch(Exception ex)
                  {
                      error =ex.Message;
                  }

                 
               
                 
              }
                  if (error.Length==0)
                  {
                    return Ok(new Response { Status = "Success", Message = " Add Was Successfull!" });
                  }
                  else
                  {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                       new Response { Status = "Error", Message =error});   
                  }
           

        }


          [HttpPost]
          [Route("UploadTubePhoto")]
          [DisableRequestSizeLimit]
           public async Task <IActionResult> AddTubePhoto()
          {
            // write photos to disk 
             string error=string.Empty;
              if (Request.Form.Files.Any())
              {
                 
                  try 
                  {
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string uploadsDir = Path.Combine(webRootPath, "uploads");
                     // wwwroot/uploads/
                    if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                     // tube picture 
                     IFormFile file = Request.Form.Files[0];
                      var batchId= file.Name;
                   //  string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).file.Trim('"');
                     string fileName = batchId+"_Tube.jpg";
                      string fullPath = Path.Combine(uploadsDir, fileName);
                         var buffer = 1024 * 1024;
                      using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer, useAsync: false);
                      await file.CopyToAsync(stream);
                     await stream.FlushAsync();    
                  }
                  catch(Exception ex)
                  {
                      error =ex.Message;
                      
                  }
  
              }
                  if (error.Length==0)
                  {
                    return Ok(new Response { Status = "Success", Message = " Add Was Successfull!" });
                  }
                  else
                  {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                       new Response { Status = "Error", Message =error});   
                  }
           
          }













    }
}