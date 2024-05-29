using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.BusinessLogic
{
    public interface IBatchBL
    {
       Task <bool> SendBatch(int Id);
    }
}