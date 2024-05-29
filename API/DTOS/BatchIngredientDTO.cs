using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class BatchIngredientDTO
    {
        public int Id { get; set; }

        public int BatchId { get; set; }
        
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } 

        //added
         public string IngredientCode { get; set; } 

        public decimal QTYPerTube { get; set; }

        
        public decimal QTYPerBatch { get; set; } 

         public bool IsChecked { get; set; }
    }
}