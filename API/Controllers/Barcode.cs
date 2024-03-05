using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Barcode : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public Barcode(DataContext dataContext,IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = dataContext;   
        }  
        [HttpGet("{barcode}")]
        [Authorize]
        public async Task<ActionResult<BarcodeForViewDTO>> GetBarcode(string barcode)
        {
             var _result = await _dataContext.Barcodes.Where(x=>x.barcode==barcode)
             .Include(x=>x.product)
             .FirstOrDefaultAsync();
             if (_result==null)
             {
                return NotFound();
             }
             var barcodeforviewdto = _mapper.Map<BarcodeForViewDTO>(_result);
             return Ok(barcodeforviewdto);
        }

        [HttpGet]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult< IEnumerable<BarcodeForViewDTO>>> getBarcodes()
        {
             var barcodes = await _dataContext.Barcodes.Include(x=>x.product).ToListAsync();
             var barcodesForViewDTOs = _mapper.Map<IEnumerable< BarcodeForViewDTO>>(barcodes);
             return Ok(barcodesForViewDTOs) ;
        }

        [HttpPost]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task <IActionResult> AddBarcode([FromBody] BarcodeForViewDTO barcodeForViewDTO)
        {
            var barcode = _mapper.Map<API.Entities.Barcode>(barcodeForViewDTO);
            var result = _dataContext.Barcodes.Add(barcode);
            await _dataContext.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Barcode Add Was Successfull!" });

        }

         [HttpPut("{Id}")]
           [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> Update ([FromBody]BarcodeForViewDTO barcodeForViewDTO,int Id)
        {
            var barcode = _mapper.Map<API.Entities.Barcode>(barcodeForViewDTO);
            // update ... keep in the controller for now 
            var originalEntity =await _dataContext.Barcodes.FirstOrDefaultAsync(x=>x.Id==Id);
            if (originalEntity!=null)
            {
                originalEntity.barcode = barcodeForViewDTO.barcode;
                originalEntity.NDCNO = barcodeForViewDTO.NDCNO;
                originalEntity.ProductId = barcodeForViewDTO.ProductId;
                originalEntity.TubeWeight  = barcodeForViewDTO.TubeWeight;
                _dataContext.Entry(originalEntity).Property(x=>x.barcode).IsModified=true;
                _dataContext.Entry(originalEntity).Property(x=>x.NDCNO).IsModified=true;
                _dataContext.Entry(originalEntity).Property(x=>x.ProductId).IsModified=true;
                _dataContext.Entry(originalEntity).Property(x=>x.TubeWeight).IsModified=true;

                var result = await _dataContext.SaveChangesAsync();
            if (result==0)
           {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Update Failed!"});            
           }
            
             return Ok(new Response { Status = "Success", Message = "Update Was Successfull!" });
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError,
            new Response { Status = "Error", Message = "Original Entity Not Found"});      

          

            
        }

        [HttpDelete("{Id}")]
          [Authorize(Policy ="ManagerPolicy")]
        public  ActionResult Delete (int Id)
        {
            // find another (better) way than try catch
            try 
            {
            var originalEntity= _dataContext.Barcodes.FirstOrDefault(x=>x.Id==Id);
            if (originalEntity!=null)
            {
              _dataContext.Barcodes.Remove(originalEntity);
              _dataContext.SaveChanges();
            }
              
              return NoContent();
            }
            catch(Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message =ex.Message});           
            }
            
        }


    }
}