using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Notification
    {

        public int Id { get; set; }

        [ForeignKey (nameof(User))]
        public string UserId { get; set; }

        public  virtual User User { get; set; }

        public string NotificationMessage { get; set; }

        public int? BatchTaskId { get; set; }

        public BatchTask BatchTask { get; set; }

        // for global notification that batch is started or ended ...
        // there is no interaction from the user ...
        public int? BatchId { get; set; }

        public string AssignedByUserId { get; set; }

        public bool IsRead { get; set; } 

        public DateTime? DateSent { get; set; }

        public DateTime? DateRead { get; set; }

        
    }
}