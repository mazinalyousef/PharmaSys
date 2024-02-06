using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string IngredientName { get; set; }


       // note : may change to ICollection....
         public List<ProductIngredient> ProductIngredients { get; set; }
         public ICollection<BatchIngredient> BatchIngredients { get; set; }
    }
}