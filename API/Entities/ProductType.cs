using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ProductType
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}