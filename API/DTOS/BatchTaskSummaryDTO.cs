using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{

    public class BatchTaskSummaryDTO
    {

        public int Id { get; set; }
        public int TaskTypeId { get; set; }
        public string TaskTitle { get; set; }

        public  string TaskState { get; set; }

        public string User { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Totalminutes { get; set; }

    }
    
}