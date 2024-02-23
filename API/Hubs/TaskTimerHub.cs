using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    [Authorize]
    public class TaskTimerHub:Hub
    {
        public async Task AddUserToGroup(int groupId)
        {
             await Groups.AddToGroupAsync(Context.ConnectionId,groupId.ToString());
        }
    }
}