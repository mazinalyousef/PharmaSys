using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using API.Hubs;
using Microsoft.AspNetCore.Identity;
using API.Helpers;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Interfaces
{
    public class MessageRepository : IMessageRepository
    {
         private readonly DataContext _dataContext;
         private readonly UserManager<User> _userManager;
         
         public MessageRepository(DataContext dataContext,UserManager<User> userManager)
         {
            _dataContext = dataContext;
             _userManager = userManager;
            
         }
        public async Task<bool> Add(Message message)
        {
             // we want to set the date sent and the managers Ids ....
             // so we can send a message foeach manager...
              
              // get all managers 
              bool completed =false;
              var ManagerUsers=  await _userManager.GetUsersInRoleAsync(UserRoles.Manager);  

              using (IDbContextTransaction transaction =await _dataContext.Database.BeginTransactionAsync())
              {      

                     try
                     {

                      foreach (var userItem in ManagerUsers)
                      {
                           Message newMessage= new Message
                           {
                              UserId = message.UserId,
                              BatchTaskId = message.BatchTaskId,
                              BatchId = message.BatchId,
                              DateSent = DateTime.Now,
                               DestinationUserId = userItem.Id,
                               MessageText = message.MessageText,
                           };

                         _dataContext.Messages.Add(newMessage);
                          await _dataContext.SaveChangesAsync();
                      }
                     

                     await transaction.CommitAsync();
                     completed=true;
                     }
                     catch(Exception )
                     {
                       await transaction.RollbackAsync();
                     
                     }
                    
              }  
            
             return  completed;
        }

        public async Task<Message> Get(int Id)
        {
              return await _dataContext.Messages
              .FirstOrDefaultAsync(x=>x.Id==Id);
        }

        public async Task<IEnumerable<Message>> GetAllForUser(string UserId)
        {
             var IQR= _dataContext.Messages.Include(x=>x.User)
             .Include(x=>x.BatchTask).ThenInclude(x=>x.Batch)
             .Where(x=>x.DestinationUserId==UserId).OrderByDescending(x=>x.Id).Take(50);

           return await IQR.ToListAsync();
        }

         public async Task<IEnumerable<Message>> GetAllUnreadForUser(string UserId)
        {
           
         return await _dataContext.Messages
        .Where(x=>x.DestinationUserId==UserId&&x.IsRead==false).ToListAsync();

        }
		

         public async Task<bool> SetAllAsRead(string userId)
        {
         var originalEntities= await _dataContext.Messages.Where
         (x=>x.DestinationUserId==userId).ToListAsync();
         foreach(var item in originalEntities)
         {
          item.IsRead=true;
          _dataContext.Entry(item).Property(x=>x.IsRead).IsModified=true;
         }
          await _dataContext.SaveChangesAsync();

          return true;
        }
    }
}