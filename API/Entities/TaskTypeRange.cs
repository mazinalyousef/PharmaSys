using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
     
    public class TaskTypeRange
    {

        public TaskTypeRange( )
        {
            
        }

         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
         public int Id { get; set; }

        [Column(TypeName = "decimal(32,3)")]
        public decimal RangeValue { get; set; }

        public int TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }

        public int? TaskTypeGroupId { get; set; }

        #nullable enable
        public TaskTypeGroup? TaskTypeGroup { get; set; }

       

    }
}