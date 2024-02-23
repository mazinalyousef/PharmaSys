using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IBatchRepository
    {
         // Task<IEnumerable<Batch>> getBatches();
       Task<IEnumerable<Batch>> GetAll();

       Task<Batch> Get(int Id);

       Task<Batch> GetBatch(int Id);

       int getBatchState(int Id);

       bool setBatchState(int id,int newSate);

       Task<int> Add (Batch batch);

        Task<int> Add_Batch_Dataset(Batch batch);
        

       Task<Batch> Update(int Id,Batch batch);


        Task<bool> SetBatchAsStarted(int Id);
        
       

       void Delete(int Id);

     
       



    }
}