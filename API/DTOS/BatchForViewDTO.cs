using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class BatchForViewDTO
    {

        
            public int Id { get; set; }
            public string BatchNO { get; set; }
            public decimal BatchSize {get;set;} 

            public string NDCNO { get; set; } 

            public string ProductName { get; set; }
    }
}