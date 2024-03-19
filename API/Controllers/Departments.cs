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

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Departments : ControllerBase
    {
         private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        
         public Departments(DataContext dataContext, IConfiguration configuration,IMapper mapper)
        {
            _dataContext = dataContext;
             _mapper = mapper;
            _configuration = configuration;
        }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetBatches()
        {
                  
                var departments = await _dataContext.Departments.ToListAsync();
                var departmentforView = _mapper.Map<IEnumerable<DepartmentDTO>>(departments);
                return Ok(departmentforView) ;
                
        }   
        
    }
}