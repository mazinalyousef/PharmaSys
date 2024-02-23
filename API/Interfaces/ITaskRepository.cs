using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOS;
using API.Entities;


namespace API.Interfaces
{
    public interface ITaskRepository
    {
        Task<int> Add (BatchTask batchTask);

        Task<bool> SetAsAssigned(int TaskId,string UserId);

        Task<bool> SetAsCompleted(int TaskId);


        Task<BatchTask> getBatchTaskInfo(int Id);

        BatchTask GetBatchTask(int _batchId,int _taskIdtypeId,int _departmentId);

        Task<CheckedListTaskForViewDTO> getCheckedListTaskForView(int Id);

        Task<RawMaterialsTaskForViewDTO> GetRawMaterialsTaskForView(int Id);

         
         Task<RangeSelectTaskForViewDTO> GetRangeSelectTaskForViewDTO(int Id);


         List<BatchTask> getBatchTasks(int _batchId);
          

          Task<IEnumerable<BatchTask>>GetUserRunningTasks(string userId);
        
        
    }
}