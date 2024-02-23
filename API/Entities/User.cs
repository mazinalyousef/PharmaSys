using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class User :IdentityUser
    {
public User( )
{
       Batches=new HashSet<Batch>();
       BatchTasks =new HashSet<BatchTask>();
}
      #nullable enable
      public string FullName { get; set; }
      
      public int? DepartmentId { get; set; }
      public Department? Department { get; set; }
    
      public ICollection<Batch> Batches { get; set; }
      public ICollection<BatchTask> BatchTasks { get; set; }

      public ICollection <Notification> Notifications { get; set; }

      //added
      public ICollection <Notification> AssignedNotifications { get; set; }
    }
}