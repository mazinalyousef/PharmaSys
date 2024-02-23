using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class ProductIngredientDTO
    {
         public int ProductId { get; set; }
        
         public int IngredientId { get; set; }
         public  string IngredientTitle { get; set; }
        
         public decimal Percentage { get; set; }
    }
}