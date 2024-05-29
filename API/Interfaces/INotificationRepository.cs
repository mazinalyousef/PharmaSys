using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>>GetAllForUser(string UserId);

         Task<IEnumerable<Notification>>GetAllUnreadForUser(string UserId);
        Task<IEnumerable<Notification>> GetAll(string userId);
       Task<Notification> Get(int Id);
       Task<int> Add (Notification notification);

       Task<bool> SetAllAsRead(string userId);
       void Delete(int Id);
    }
}