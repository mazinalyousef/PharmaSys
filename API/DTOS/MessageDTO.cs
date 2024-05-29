using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class MessageDTO
    {
        public int Id { get; set; }

        
        public string UserId { get; set; }
        
        public string UserName{get;set;}
        public string MessageText { get; set; }

        public int? BatchTaskId { get; set; }
        
        
        public int? BatchId { get; set; }
        public string BatchNO{get;set;}

        public string DestinationUserId { get; set; }

        public bool IsRead { get; set; } 

        public DateTime? DateSent { get; set; }

        public DateTime? DateRead { get; set; }
    }
}