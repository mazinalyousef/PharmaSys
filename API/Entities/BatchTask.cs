using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{


    public class BatchTask
    {

      public BatchTask()
      {
        
        BatchTaskRanges=new HashSet<BatchTaskRange>();
        BatchTaskNotes=new HashSet<BatchTaskNote>();
        BatchTaskCertificates=new HashSet<BatchTaskCertificate>();
      }
        public int Id { get; set; }


        public int TaskStateId { get; set; }
        public TaskState TaskState { get; set; }


         public int TaskTypeId { get; set; }
        public TaskType TaskType { get; set; }


        // re-check if this nullable or not ....
        public int BatchId { get; set; }
        public Batch Batch { get; set; }


          public int? DepartmentId { get; set; }

         #nullable enable
         public Department? Department { get; set; }

            #nullable enable
          public string? UserId { get; set; }

           #nullable enable
           public User? User { get; set; }

          public int DurationInSeconds { get; set; }
           
          public DateTime? StartDate { get; set; }
          public DateTime? EndDate { get; set; }
  
          
         

            public ICollection<BatchTaskRange> BatchTaskRanges { get; set; }


             public ICollection<BatchTaskNote> BatchTaskNotes { get; set; }
          
              public ICollection<BatchTaskCertificate> BatchTaskCertificates { get; set; }


                public ICollection<Notification> Notifications { get; set; }
    }
}