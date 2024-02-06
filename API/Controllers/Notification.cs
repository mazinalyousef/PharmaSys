using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetUnreadNotifications(string Id)
        {
            var notifications  = await _notificationRepository.GetAllUnreadForUser(Id);
            var notificationsForView = _mapper.Map<IEnumerable< NotificationDTO>>(notifications);
            return Ok(notificationsForView) ;
         
        }
    }
}