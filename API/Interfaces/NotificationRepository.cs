using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly DataContext _dataContext;

        public NotificationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            
        }
        public async Task<int> Add(Notification notification)
        {
          var result=  _dataContext.Notifications.Add(notification);
          await _dataContext.SaveChangesAsync();
           return result.Entity.Id;
        }


        public void Delete(int Id)
        {
          var originalEntity= _dataContext.Notifications.FirstOrDefault(x=>x.Id==Id);
          if (originalEntity!=null)
          {
            _dataContext.Notifications.Remove(originalEntity);
              _dataContext.SaveChangesAsync();
          }
        }

        public async Task<Notification> Get(int Id)
        {
             return await _dataContext.Notifications
            .FirstOrDefaultAsync(x=>x.Id==Id);
        }

        public async Task<IEnumerable<Notification>> GetAll(string userId)
        {
            return await _dataContext.Notifications.Where(x=>x.UserId==userId).ToListAsync();
           
        }

        public async Task<IEnumerable<Notification>> GetAllForUser(string UserId)
        {
          //return await _dataContext.Notifications.Where(x=>x.UserId==UserId &&
         //  x.IsRead==false).ToListAsync();

          //   return await _dataContext.Notifications.Include(x=>x.User)
          //   .Where(x=>x.UserId==UserId).ToListAsync();

           return await _dataContext.Notifications.Include(x=>x.AssignedByUser)
           .Where(x=>x.UserId==UserId).ToListAsync();

        }
         public async Task<IEnumerable<Notification>> GetAllUnreadForUser(string UserId)
        {
          //return await _dataContext.Notifications.Where(x=>x.UserId==UserId &&
         //  x.IsRead==false).ToListAsync();

           //  return await _dataContext.Notifications.Include(x=>x.User)
          //   .Where(x=>x.UserId==UserId&&x.IsRead==false).ToListAsync();

       return await _dataContext.Notifications.Include(x=>x.AssignedByUser)
        .Where(x=>x.UserId==UserId&&x.IsRead==false).ToListAsync();

        }

        public async Task<bool> SetAllAsRead(string userId)
        {
         var originalEntities= await _dataContext.Notifications.Where(x=>x.UserId==userId).ToListAsync();
         foreach(var item in originalEntities)
         {
          item.IsRead=true;
          _dataContext.Entry(item).Property(x=>x.IsRead).IsModified=true;
         }
          await  _dataContext.SaveChangesAsync();

          return true;
        }
    }
}