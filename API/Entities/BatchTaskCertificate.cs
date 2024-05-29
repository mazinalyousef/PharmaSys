using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class BatchTaskCertificate
    {
        public int Id { get; set; }

        public int BatchTaskId { get; set; }

        public BatchTask BatchTask { get; set; }

        public string Title { get; set; }
    }
}