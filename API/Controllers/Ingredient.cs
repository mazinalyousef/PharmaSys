using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using AutoMapper;
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

        [HttpGet]
        public async Task<ActionResult< IEnumerable<IngredientDTO>>> getIngredients()
        {
             var ingredients = await _dataContext.Ingredients.ToListAsync();
             var ingredientsforviewDTOS = _mapper.Map<IEnumerable< IngredientDTO>>(ingredients);
             return Ok(ingredientsforviewDTOS) ;
        }

        [HttpPost]
        public async Task <IActionResult> Add([FromBody] IngredientDTO ingredientDTO)
        {
            var item = _mapper.Map<API.Entities.Ingredient>(ingredientDTO);
            var result = _dataContext.Ingredients.Add(item);
            await _dataContext.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "Add Was Successfull!" });

        }

         [HttpPut("{Id}")]
        public async Task<IActionResult> Update ([FromBody]IngredientDTO ingredientDTO,int Id)
        {
            var item = _mapper.Map<API.Entities.Ingredient>(ingredientDTO); // not necessary code...
            // update ... keep in the controller for now 
            var originalEntity =await _dataContext.Ingredients.FirstOrDefaultAsync(x=>x.Id==Id);
            if (originalEntity!=null)
            {
                originalEntity.IngredientName = ingredientDTO.IngredientName;
                _dataContext.Entry(originalEntity).Property(x=>x.IngredientName).IsModified=true;
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