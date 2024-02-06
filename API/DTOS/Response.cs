using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class Response
    {
        #nullable enable
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}