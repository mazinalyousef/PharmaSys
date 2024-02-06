using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TaskType
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public int DurationInSeconds { get; set; }

        //DepartmentId property  stored here and stored in batch Task 
        // the reason to store it here is  it acts like a predefined template 
        // to exctract the department Id from here (database stored value...)....
        public int? DepartmentId { get; set; }

      public ICollection<TaskTypeGroup> TaskTypeGroups { get; set; }
      public ICollection<TaskTypeCheckList> TaskTypeCheckLists { get; set; }
      public ICollection<TaskTypeRange> taskTypeRanges { get; set; }
      public ICollection<BatchTask> BatchTasks { get; set; }
    }
}