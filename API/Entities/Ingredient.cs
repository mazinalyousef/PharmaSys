using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        public string IngredientName { get; set; }

        [StringLength(250)]
        public string IngredientCode{get;set;}


       // note : may change to ICollection....
         public List<ProductIngredient> ProductIngredients { get; set; }
         public ICollection<BatchIngredient> BatchIngredients { get; set; }
    }
}