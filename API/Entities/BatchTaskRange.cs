using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class BatchTaskRange
    {
         public int Id { get; set; }

        public int BatchTaskId { get; set; }

        public BatchTask BatchTask { get; set; }




         public int? TaskTypeRangeId { get; set; }


        
        // optional feild ... may drop later... 
        public string TaskTypeRangeTitle { get; set; }

        [Column(TypeName = "decimal(32,2)")]
        public decimal TaskTypeRangeValue { get; set; }
    }
}