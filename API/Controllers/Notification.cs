using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize]
    public class Notification : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        public Notification(INotificationRepository notificationRepository,
        IMapper mapper)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
           
        }

         [HttpGet("{Id}")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUserNotifications(string Id)
        {
            var notifications  = await _notificationRepository.GetAllForUser(Id);
            var notificationsForView = _mapper.Map<IEnumerable< NotificationDTO>>(notifications);
            return Ok(notificationsForView) ;
         
        }

        
         [HttpGet]
        [Route("Unread/{Id}")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUserUnreadNotifications(string Id)
        {
            var notifications  = await _notificationRepository.GetAllUnreadForUser(Id);
            var notificationsForView = _mapper.Map<IEnumerable< NotificationDTO>>(notifications);
            return Ok(notificationsForView) ;
        }

         [HttpPut("{UserId}")]
        public async Task<IActionResult> SetAsRead (string UserId)
        {
              var result= await _notificationRepository.SetAllAsRead(UserId);
              if (!result)
              {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = " Update Failed!"});            
              }

              return Ok(new Response { Status = "Success", Message = " Update Was Successfull!" });
        }
    }
}