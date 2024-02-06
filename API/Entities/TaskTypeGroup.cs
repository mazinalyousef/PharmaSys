using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{

    
    public class TaskTypeGroup
    {
        public int Id { get; set; }

        public string Title { get; set; }

         public int TaskTypeId { get; set; }
         public TaskType TaskType { get; set; }

         public ICollection<TaskTypeCheckList> TaskTypeCheckLists { get; set; }
          public ICollection<TaskTypeRange> taskTypeRanges { get; set; }
    }
}