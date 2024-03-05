using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Barcode
    {
     public int Id { get; set; }  



     [Required]
     public string barcode { get; set; }


      public int ProductId { get; set; }
      public Product product { get; set; }

      public string NDCNO { get; set; } 

      [Column(TypeName = "decimal(32,3)")]
      public decimal TubeWeight {get;set;} 

    }
}