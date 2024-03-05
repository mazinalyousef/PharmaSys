using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;


namespace API.Hubs
{

    [Authorize]
    public class MessageHub :Hub
    {
         private readonly UserManager<User> _userManager;

         public MessageHub(UserManager<User> userManager)
         {
             _userManager = userManager;
         }

          public async Task AddUserToGroup(string userName)
        {
            
            //get roles ...
            var userEntity=  await _userManager.FindByNameAsync(userName);
            var userroles   = await _userManager.GetRolesAsync(userEntity);
             

             
            // will add the user to his related groups (based on user roles)
            foreach(string role in userroles)
            {
               await Groups.AddToGroupAsync(Context.ConnectionId,role);
              
            }
        }
 
        public async Task SendNote(string groupName,string mesasge)
        {
               await Clients.Group(groupName).SendAsync("ReceiveNote",mesasge);

        }
        
    }
}