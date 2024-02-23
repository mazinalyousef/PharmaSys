using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class NotificationDTO
    {
         public int Id { get; set; }
        public string UserId { get; set; }
        public string NotificationMessage { get; set; }
        public int? BatchTaskId { get; set; }
         
        // for global notification that batch is started or ended ...
        // there is no interaction from the user ...
        public int? BatchId { get; set; }
        public string AssignedByUserId { get; set; }

        
         public string TakenDisplayTitle { get; set; }
        public bool IsRead { get; set; } 
        public DateTime? DateSent { get; set; }
        public DateTime? DateRead { get; set; }
    }
}