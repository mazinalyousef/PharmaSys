using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class InsertedEntityStatus
    {
        public int InsertedId { get; set; }
        public bool OperationsSucceeded { get; set; }
    }
}