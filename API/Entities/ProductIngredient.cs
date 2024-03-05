using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ProductIngredient
    {

        public int ProductId { get; set; }
        public  Product Product { get; set; }

        public int IngredientId { get; set; }
         public  Ingredient Ingredient { get; set; }


         [Column(TypeName = "decimal(32,3)")]
         public decimal Percentage { get; set; }

         

    }
}