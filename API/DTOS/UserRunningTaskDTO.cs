using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class UserRunningTaskDTO
    {
        public int Id { get; set; }
        public int TaskStateId { get; set; }
        public int TaskTypeId { get; set; }


        public string TaskTypeTitle { get; set; }


        
         public int BatchId { get; set; }

         public string BatchNO { get; set; }
         public decimal BatchSize {get;set;} 
        public string ProductName { get; set; }
         public decimal TubeWeight {get;set;} 
         public int TubesCount { get; set; }

        #nullable enable
        public string? UserId { get; set; }

        #nullable enable
         

         public int DurationInSeconds { get; set; }
           
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; } 
    }
}