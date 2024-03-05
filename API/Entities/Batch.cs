using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Batch
    {
        
        public int Id { get; set; }

        public string BatchNO { get; set; }

        /// <summary>
        /// The Batch Size ... In Kilogram 
        /// </summary>
        /// 
        [Column(TypeName = "decimal(32,3)")]
        public decimal BatchSize {get;set;} 

        public DateTime?  MFgDate { get; set; } 
         public DateTime? ExpDate { get; set; }

        public string Revision { get; set; }    
        public DateTime? RevisionDate { get; set; }

        public string Barcode { get; set; } 

        public string MFNO { get; set; } 


         
        public string NDCNO { get; set; } 



        public int ProductId { get; set; }
        public Product Product { get; set; }

        [NotMapped]
        public string ProductName { get; set; }



        public string UserId { get; set; }
        public User User { get; set; }


        public int BatchStateId { get; set; }

        public BatchState BatchState { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string TubePictureURL { get; set; }

        public string CartoonPictureURL { get; set; }

        [Column(TypeName = "decimal(32,2)")]
        public decimal TubeWeight {get;set;} 

        public int TubesCount { get; set; }

        public ICollection<BatchIngredient> BatchIngredients { get; set; }

       
        
        
    }
}