using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings =false)]
        public string ProductName { get; set; }

        public int? ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }


       // note : may change to ICollection....
        public List<ProductIngredient> ProductIngredients { get; set; }

        public ICollection<Batch> Batches { get; set; }

        public ICollection<Barcode>  Barcodes {get;set;}

        public string TubePictureURL { get; set; }
        public string CartoonPictureURL { get; set; }
        
    }
}