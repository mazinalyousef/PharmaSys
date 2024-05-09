using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class BarcodeForViewDTO
    {

      
      public int Id { get; set; }  

     public string barcode { get; set; }


      public int ProductId { get; set; }
      
      public string ProductName { get; set; }

      public string NDCNO { get; set; } 

      
      public decimal TubeWeight {get;set;} 

    }
}