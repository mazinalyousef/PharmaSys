using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOS
{
    public class ProductDTO
    {
        public int Id { get; set; }
        
        public string ProductName { get; set; }

         public int? ProductTypeId { get; set; }

        public string ProductTypeTitle { get; set; }

       // note : may change to ICollection....
        public List<ProductIngredientDTO> ProductIngredients { get; set; }
    }
}