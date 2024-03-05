using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Message
    {
         public int Id { get; set; }

        [ForeignKey (nameof(User))]
        public string UserId { get; set; }

        public  virtual User User { get; set; }

        public string MessageText { get; set; }

        public int? BatchTaskId { get; set; }

        public BatchTask BatchTask { get; set; }

        
        public int? BatchId { get; set; }

        public string DestinationUserId { get; set; }
        //added
        public  virtual User DestinationUser { get; set; }

        public bool IsRead { get; set; } 

        public DateTime? DateSent { get; set; }

        public DateTime? DateRead { get; set; }

    }
}