using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.BusinessLogic
{
    public interface ITaskBL
    {
         bool SetAsCompleted(int Id);
    }
}