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
    public class NotificationHub : Hub
    {

        private readonly UserManager<User> _userManager;
        

        public NotificationHub(UserManager<User> userManager)
        {
          
            _userManager = userManager;
            
        }
        public async Task AddUserToGroup(string userName)
        {
            // string userName= Context.User.Identity.Name;
            //get roles ...
            var userEntity=  await _userManager.FindByNameAsync(userName);
            var userroles   = await _userManager.GetRolesAsync(userEntity);
             
           // will add the user to his related groups (based on user roles)
            foreach(string role in userroles)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId,role);
            }

           // and will listen to receive notification event  (should be outside the onconnected??)
            // for now the listen event is on the client --
            // -- and the responsibility of send message to group is outside here (may be from send batch ...)--
            // -- so keep empty for now 

        }

         public async Task SendNotificationMessage(string groupName,string mesasge)
        {
               await Clients.Group(groupName).SendAsync("ReceiveMessage",mesasge);

        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("UserIsOnline",Context.ConnectionId);
           // return base.OnConnectedAsync();
          
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
             await Clients.Others.SendAsync("UserIsOffline",Context.ConnectionId);
           // return base.OnDisconnectedAsync(exception);

            // will remove the user from his related groups (based on user roles)
            // this may be done automatically with signal R

            // and will cancel listen to receive notification event  (if needed)

           await base.OnDisconnectedAsync(exception);
        }
    }
}