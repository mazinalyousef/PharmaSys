using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class TaskTypeRangeDTO
    {
        public int Id { get; set; }
        public decimal RangeValue { get; set; }
        public int TaskTypeId { get; set; }
        
        public int? TaskTypeGroupId { get; set; }
        public  string TaskTypeGroupTitle { get; set; }   
    }
}