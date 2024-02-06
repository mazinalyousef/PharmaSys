using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class BatchState
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public ICollection<Batch> Batches { get; set; }
    }
}