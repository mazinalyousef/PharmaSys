using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOS
{
    public class EMailMessageDTO
    {
        

    public string To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    }
}