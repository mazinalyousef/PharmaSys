using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOS
{
    public class BatchForEditDTO
    {
        
        public int Id { get; set; }
        public string BatchNO { get; set; }

        public decimal BatchSize {get;set;} 

        public DateTime?  MFgDate { get; set; } 
         public DateTime? ExpDate { get; set; }

        public string Revision { get; set; }    
        public DateTime? RevisionDate { get; set; }

        public string Barcode { get; set; } 

        public string MFNO { get; set; } 
         
        public string NDCNO { get; set; } 

        public int ProductId { get; set; }

        // added...
         public string ProductName { get; set; }
        
        public string UserId { get; set; }
       
        public int BatchStateId { get; set; }
       
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string TubePictureURL { get; set; }

        public string CartoonPictureURL { get; set; }

        public decimal TubeWeight {get;set;} 

        public int TubesCount { get; set; }


         public ICollection<BatchIngredientDTO> BatchIngredients { get; set; }



    }
}