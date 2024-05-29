using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class BatchTaskInfoDTO
    {
        public int Id { get; set; }
        public int TaskStateId { get; set; }

         public int TaskTypeId { get; set; }
        
        // re-check if this nullable or not ....
        public int BatchId { get; set; }
    
          public int? DepartmentId { get; set; }
            #nullable enable
          public string? UserId { get; set; }

           #nullable enable
        
          public int DurationInSeconds { get; set; }
           
          public DateTime? StartDate { get; set; }
          public DateTime? EndDate { get; set; }
    }
}