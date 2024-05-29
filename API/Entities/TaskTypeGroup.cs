using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{

    
    public class TaskTypeGroup
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Title { get; set; }

         public int TaskTypeId { get; set; }
         public TaskType TaskType { get; set; }

         public ICollection<TaskTypeCheckList> TaskTypeCheckLists { get; set; }
          public ICollection<TaskTypeRange> taskTypeRanges { get; set; }
    }
}