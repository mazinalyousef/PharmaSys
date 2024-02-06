using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Department
    {
         public int Id { get; set; }
         public string Title { get; set; }

         public ICollection<User> Users { get; set; }
         public ICollection<BatchTask> BatchTasks { get; set; }
    }
}