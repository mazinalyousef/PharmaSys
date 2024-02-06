using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class RawMaterialsTaskForViewDTO
    {


        public RawMaterialsTaskForViewDTO()
        {

            batchIngredientDTOs =new HashSet<BatchIngredientDTO>();
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

         public ICollection<BatchIngredientDTO>  batchIngredientDTOs {get;set;}
    }
}