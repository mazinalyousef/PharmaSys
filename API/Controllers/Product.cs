using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.DTOS;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class Product : ControllerBase
    {
      
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public Product(IProductRepository productRepository,IMapper mapper)
        {
            _productRepository =productRepository;
            _mapper= mapper;
          
        }

        [HttpDelete("{Id}")]
          [Authorize(Policy ="ManagerPolicy")]
        public  ActionResult Delete (int Id)
        {
            // find another (better) way than try catch
            try 
            {
              _productRepository.Delete(Id);
              
              return NoContent();
            }
            catch(Exception ex)
            {
                 return StatusCode(StatusCodes.Status500InternalServerError,
                 new Response { Status = "Error", Message =ex.Message});           
            }
            
        }

        [HttpGet("{Id}")]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int Id)
        {
             var _result = await _productRepository.GetWithIngredients(Id);
            
             if (_result==null)
             {
                return NotFound();
             }
             var productDTO = _mapper.Map<ProductDTO>(_result);
             return Ok(productDTO);
        }

        [HttpGet]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult< IEnumerable<ProductDTO>>> getProducts()
        {
             var productsDTO = await _productRepository.GetAll();
              
             return Ok(productsDTO) ;
        }

        [HttpPost]
        
          [Authorize(Policy ="ManagerPolicy")]
        public async Task <IActionResult> AddProduct([FromBody] ProductDTO productDTO)
        {

           
            var product = _mapper.Map<API.Entities.Product>(productDTO);
             
            var result = await _productRepository.Add_Product_Dataset(product);

           
            if (result.OperationsSucceeded)
            {
            return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Insert Failed!"});   
            }
           

        }

          [HttpPut("{Id}")]
           [DisableRequestSizeLimit]
            [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> Update ([FromBody]ProductDTO productDTO,int Id)
        {
            var product = _mapper.Map<API.Entities.Product>(productDTO);
            // update ... keep in the controller for now 
           bool success= await _productRepository.Update_Product_Dataset(Id,product);
           if (success)
           {
                 return Ok(new Response { Status = "Success", Message = "Update Was Successfull!" });
           }
           else
           {  return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Update Failed!"});         }
           
        }
    }
}