using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Ingredient : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public Ingredient(DataContext dataContext,IMapper mapper)
        {
             _mapper = mapper;
            _dataContext = dataContext;   
        }


        [HttpGet("{Id}")]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<IngredientDTO>> GetIngredient(int Id)
        {
             var _result = await _dataContext.Ingredients.Where(x=>x.Id==Id) 
             .FirstOrDefaultAsync();
             if (_result==null)
             {
                return NotFound();
             }
             var ingredientForViewDTO = _mapper.Map<IngredientDTO>(_result);
             return Ok(ingredientForViewDTO);
        }

      
        // original version .. without params
     
        [HttpGet]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult< IEnumerable<IngredientDTO>>> getIngredients()
        {
             var ingredients = await _dataContext.Ingredients.ToListAsync();
             var ingredientsforviewDTOS = _mapper.Map<IEnumerable< IngredientDTO>>(ingredients);
             return Ok(ingredientsforviewDTOS) ;
        }
     



        // modified version   .. with params ...
        [HttpGet("PagedList")]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult< PagedList<IngredientDTO>>> getIngredients([FromQuery]UserParams userParams)
        {
             var ingredients = await _dataContext.Ingredients.ToListAsync();

             var query = _dataContext.Ingredients.
             ProjectTo<IngredientDTO>(_mapper.ConfigurationProvider)
             .AsNoTracking();

            var pagedList= await PagedList<IngredientDTO>.CreateAsync(query,userParams.PageNumber,userParams.PageSize);
             Response.AddPaginationHeader(pagedList.CurrentPage,pagedList.PageSize,pagedList.TotalCount,pagedList.TotalPages);
           return  Ok(pagedList);
        }

        

        [HttpPost]
          [Authorize(Policy ="ManagerPolicy")]
        public async Task <IActionResult> Add([FromBody] IngredientDTO ingredientDTO)
        {
            var item = _mapper.Map<API.Entities.Ingredient>(ingredientDTO);
            var result = _dataContext.Ingredients.Add(item);
            await _dataContext.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Add Was Successfull!" });

        }

         [HttpPut("{Id}")]
           [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> Update ([FromBody]IngredientDTO ingredientDTO,int Id)
        {
            var item = _mapper.Map<API.Entities.Ingredient>(ingredientDTO); // not necessary code...
            // update ... keep in the controller for now 
            var originalEntity =await _dataContext.Ingredients.FirstOrDefaultAsync(x=>x.Id==Id);
            if (originalEntity!=null)
            {
                originalEntity.IngredientName = ingredientDTO.IngredientName;
                _dataContext.Entry(originalEntity).Property(x=>x.IngredientName).IsModified=true;

                  originalEntity.IngredientCode = ingredientDTO.IngredientCode;
                _dataContext.Entry(originalEntity).Property(x=>x.IngredientCode).IsModified=true;


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
            var originalEntity= _dataContext.Ingredients.FirstOrDefault(x=>x.Id==Id);
            if (originalEntity!=null)
            {
              _dataContext.Ingredients.Remove(originalEntity);
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