using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOS
{
    public class CheckedListTaskForViewDTO
    {

        public CheckedListTaskForViewDTO()
        {
            // added.....
            taskTypeCheckLists=new HashSet<TaskTypeCheckList>();
        }
         public int Id { get; set; }
         public int TaskTypeId { get; set; }

         public string Title {get; set;}
         public int BatchId { get; set; }
         public int? DepartmentId { get; set; }

        
         #nullable enable
         public string? UserId { get; set; }


         public int DurationInSeconds { get; set; }

      
         public DateTime? StartDate { get; set; }
         public DateTime? EndDate { get; set; }



         public Batch BatchInfo { get; set; }
         public  Product ProductInfo { get; set; }

            
         public ICollection<TaskTypeCheckList> taskTypeCheckLists{get;set;}
    }
}