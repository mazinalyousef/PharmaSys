using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
 

namespace API.Interfaces
{
    public class GlobalDataRepository : IGlobalDataRepository
    {

        private readonly DataContext _dataContext;
        
        public GlobalDataRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            
        }
        public bool getTaskTypeTimersData()
        {
           
            bool completed=false;
            // fetch task type timers data from database
            Helpers.TaskTypesTimerData.taskTypesTimers = _dataContext.TaskTypesTimers.ToList();
            return completed;
        }
    }
}