using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class TaskAssignDTO
    {
        public string UserId { get; set; }

        public int TaskId { get; set; }


        public int Seconds{get;set;}


        public int TaskTypeId { get; set; }

        public int DepartmentId { get; set; }

        
    }
}