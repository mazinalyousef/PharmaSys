using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;
using API.Helpers;
using API.Hubs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     [Authorize]
    public class Message : ControllerBase
    {

        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<MessageHub> _messageHub;
        
        public Message(IMessageRepository messageRepository,IMapper mapper,IHubContext<MessageHub> messageHub )
        {
            _mapper= mapper;
            _messageRepository=messageRepository;
              _messageHub=messageHub;
        }

        [HttpGet("{Id}")]
        
        public async Task<ActionResult<IEnumerable<MessageDTO>>> getUserMessages(string Id)
        {
           var messages=await _messageRepository.GetAllForUser(Id);
           var MessagesForView = _mapper.Map<IEnumerable<MessageDTO>>(messages);
           return Ok(MessagesForView);
        } 


        [HttpPost]
        
        public async Task<IActionResult> SendMessage(MessageDTO messageDTO)
        {
            // map
             bool result=false;
             var _message = _mapper.Map<API.Entities.Message>(messageDTO);
              result =await _messageRepository.Add(_message);
              if (result)
              {
                // send message via Hub
                await _messageHub.Clients.Group(UserRoles.Manager).SendAsync("ReceiveNote","You Have New Note..");
                await  _messageHub.Clients.Group(UserRoles.Manager).SendAsync("UpdateNotes","UpdateNotes");
              }
             return Ok(new Response { Status = "Success", Message = "Insert  Was Successfull!" });

            
        }   

        [HttpPut("{UserId}")]
        public async Task<IActionResult> SetAsRead (string UserId)
        {
              var result= await _messageRepository.SetAllAsRead(UserId);
              if (!result)
              {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = " Update Failed!"});            
              }

              return Ok(new Response { Status = "Success", Message = " Update Was Successfull!" });
        }


        // to be fixed later .. there was a problem with the returned value and the json in the client expecting somthing else ...
        // so this controller switched to "GetUserUnreadMessagesCount" controller 
        // that  return the count only ....
        // ......
        /*
        [HttpGet]
        [Route("Unread/{Id}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetUserUnreadMessages(string Id)
        {
            var items  = await _messageRepository.GetAllUnreadForUser(Id);
            var itemsforview = _mapper.Map<IEnumerable< MessageDTO>>(items);
            return Ok(itemsforview) ;
        }
        */

        [HttpGet]
        [Route("UnreadCount/{Id}")]
        public async Task<ActionResult<int>> GetUserUnreadMessagesCount(string Id)
        {
            var items  = await _messageRepository.GetAllUnreadForUser(Id);
             int count=0;
             if (items!=null)
             {
              count=items.ToList().Count;
             }
            return Ok(count) ;
        }


    }
}