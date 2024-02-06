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


        Task<BatchTask> getBatchTaskInfo(int Id);

        Task<CheckedListTaskForViewDTO> getCheckedListTaskForView(int Id);

        Task<RawMaterialsTaskForViewDTO> GetRawMaterialsTaskForView(int Id);

         
         Task<RangeSelectTaskForViewDTO> GetRangeSelectTaskForViewDTO(int Id);
        
        
    }
}