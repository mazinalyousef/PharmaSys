using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
          Task<IEnumerable<Message>>GetAllForUser(string UserId);

          Task<Message> Get(int Id);
          Task<bool> Add (Message message);

          Task<bool> SetAllAsRead(string userId);

          Task<IEnumerable<Message>>GetAllUnreadForUser(string UserId);
      
    }
}