using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    /// <summary>
    /// The Ingredient of the batch that will be generated after saving the batch 
    /// and is generated according to product Id And Batch Size and Tube NO.....
    /// </summary>
    public class BatchIngredient
    {
        public int Id { get; set; }

        public int BatchId { get; set; }

        public Batch Batch { get; set; }

        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        [Column(TypeName = "decimal(32,3)")]
        public decimal QTYPerTube { get; set; }

        [Column(TypeName = "decimal(32,3)")]
        public decimal QTYPerBatch { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
    }
}