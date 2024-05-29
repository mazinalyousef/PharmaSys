using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class BatchTaskNote
    {
        public int Id { get; set; }

        public int BatchTaskId { get; set; }

        public BatchTask BatchTask { get; set; }

        public string Note { get; set; }

        public DateTime SendDate { get; set; }

        /// <summary>
        /// currently keep as one recipient per note .....
        /// </summary>
        public int recipientId { get; set; }
    }
}