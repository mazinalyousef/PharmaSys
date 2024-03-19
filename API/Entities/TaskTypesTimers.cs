using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TaskTypesTimers
    {
        public int Id { get; set; }

        public int taskTypeId { get; set; }
        public TaskType taskType { get; set; }


        // neglecting relation for now 
        public int DepartmentId { get; set; }

        public int DurationInSeconds { get; set; }

       
        
    }
}