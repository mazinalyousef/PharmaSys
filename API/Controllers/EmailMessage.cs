using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailMessage : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        private UserManager<User> _userManager;

        public EmailMessage(IEmailSender emailSender,UserManager<User> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }


        [HttpPost]
        [Authorize]
        public ActionResult< bool> sendEmail(EMailMessageDTO eMailMessageDTO)
        {

             bool success=false;
             var message = new EMailMessage(new string[] { eMailMessageDTO.To }, eMailMessageDTO.Subject, 
             eMailMessageDTO.Content);
            success=  _emailSender.SendEmail(message);
            if (success)
            {
                return Ok(success) ;
            }
            else 
            { 
                 return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }


        // this could be placed in users controllers....
        [HttpGet]
        [Route("FirstManager")]
        [Authorize]
        public async Task<ActionResult<UserForViewDTO>>  GetFisrtManagerManager()
        {
           var managerUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Manager);
           if (managerUsers!=null)
           {

            
            var firstManager= managerUsers.FirstOrDefault();
              // manual mapping 
            UserForViewDTO firstmanagerDTO=new  UserForViewDTO();
            firstmanagerDTO.Id=firstManager.Id;
            firstmanagerDTO.Email = firstManager.Email;
            firstmanagerDTO.UserName = firstManager.UserName;
          

             return Ok(firstmanagerDTO);
           }
           return StatusCode(StatusCodes.Status500InternalServerError);
         
        }


    

        
        
    }
}