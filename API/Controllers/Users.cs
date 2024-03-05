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
using API.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class Users : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public Users( UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
      [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<IEnumerable<UserForViewDTO>>> GetUsers()
        {
                var users = await _userManager.Users.Include(x=>x.Department).ToListAsync();
                var usersForView = _mapper.Map<IEnumerable< UserForViewDTO>>(users);
               return Ok(usersForView) ;
        }

        [HttpGet("{Id}")]
         [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<User>> GetUser(string Id)
        {
             var _userResult = await _userManager.FindByIdAsync(Id);
             if (_userResult==null)
             {
                return NotFound();
             }
             return _userResult;
        }


        [HttpPost]
        [Route("register")]
         [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
           var _userResult = await _userManager.FindByNameAsync(registerDTO.Username);
            
           if (_userResult!=null)
           {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User already exists!"});            
           }

           
           User user =new ()
           {
                UserName=registerDTO.Username,
                Email =registerDTO.Email,
                DepartmentId=registerDTO.DepartmentId,
                SecurityStamp = Guid.NewGuid().ToString()
           };

           var result = await _userManager.CreateAsync(user,registerDTO.Password);

           
            
           if (!result.Succeeded)
           {
               return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed To Create a User" });
           }
           // create default role >>>>> member
             /*
            var defaultrole = _roleManager.FindByNameAsync(UserRoles.Member).Result;
            if (defaultrole != null)
            {
              IdentityResult roleresult = await  _userManager.AddToRoleAsync(user, defaultrole.Name);
            }
            */
   
          return Ok(new Response { Status = "Success", Message = "User Register Was Successfull!" });
        
        }


        [HttpPut("{Id}")]
        [Authorize(Policy ="ManagerPolicy")]
         public async Task<IActionResult> Update([FromBody] UpdateUserDTO updateDTO,string Id)
        {
            
           var _userResult = await _userManager.FindByIdAsync(Id);
            
           if (_userResult==null)
           {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "User Not exists!"});            
           }


           
          _userResult.UserName=updateDTO.Username;
          _userResult.Email = updateDTO.Email;
          _userResult.DepartmentId = updateDTO.DepartmentId;
    

           var result = await _userManager.UpdateAsync(_userResult);
            
           if (!result.Succeeded)
           {
               return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed To Update The User" });
           }

          return Ok(new Response { Status = "Success", Message = "User Update Was Successfull!" });
        
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoggedUserDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            var userResult = await _userManager.FindByNameAsync(loginDTO.Username);
            if (userResult != null && await _userManager.CheckPasswordAsync(userResult, loginDTO.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(userResult);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userResult.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                List<string>loggedUserRoles=new List<string> ();
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    loggedUserRoles.Add(userRole); 
                }

               var token = GetToken(authClaims);

                
                return Ok(new LoggedUserDTO
                {
                    Id= userResult.Id,
                    Username = userResult.UserName,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                    ,Roles=loggedUserRoles,

                });
            }
            return Unauthorized(new LoggedUserDTO());
        }


          
        [HttpPost("editRoles/{userName}")]
         [Authorize(Policy ="ManagerPolicy")]
        public async Task<IActionResult> EditRoles(string userName, string[]RoleNames) 
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) 
            {
                return BadRequest($"{userName} not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = RoleNames;

            selectedRoles = selectedRoles ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) 
            {
                return BadRequest("Failed to add to roles");
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) 
            {
                return BadRequest("Failed to remove the roles");
            }   

            return Ok(await _userManager.GetRolesAsync(user));         
        }


        [HttpGet("Roles/{userName}")]
         [Authorize(Policy ="ManagerPolicy")]
        public async Task<ActionResult<IEnumerable<RoleEditDto>>> GetUserRoles(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) 
            {
                return BadRequest($"{userName} not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            
            return Ok(userRoles) ;
        }



        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

              var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }




    }
}