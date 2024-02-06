using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces
{
    public class BatchRepository : IBatchRepository
    {
        private readonly DataContext _dataContext;

        public BatchRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            
        }
        public async Task<int> Add(Batch batch)
        {
              var result= _dataContext.Batches.Add(batch);
              // 
              await _dataContext.SaveChangesAsync();
              return  result.Entity.Id;     
        }


        // add batch data along with generating batch ingredients data , and save all data to database 
        public async Task<int> Add_Batch_Dataset(Batch batch)
        {
             
             var result= _dataContext.Batches.Add(batch);
              await _dataContext.SaveChangesAsync();
             var insertedBatchId=  result.Entity.Id;
/*

             // get product ingredients ....
             var productIngredients = _dataContext.ProductIngredients.Where(x=>x.ProductId==batch.ProductId).ToList();
             // calculate the batch tube weight
             decimal BatchTubeSize = batch.BatchSize / batch.TubesCount; // handle exceptions later...


             List<BatchIngredient> batchIngredients=new List<BatchIngredient> ();
             // now loop  through product ingredients 
             foreach (var productIngredientsItem  in productIngredients)
             {
                 BatchIngredient batchIngredient=new  BatchIngredient();
                 batchIngredient.BatchId = 0; // change to added batch Id ...
                 batchIngredient.IngredientId = productIngredientsItem.IngredientId;
                 batchIngredient.QTYPerTube = productIngredientsItem.Percentage;
                 batchIngredient.QTYPerBatch = BatchTubeSize *  (batchIngredient.QTYPerTube/100m);
                 batchIngredients.Add(batchIngredient);
             }
*/



           var batchIngredients = this.generate_Batch_ingredient(batch.ProductId,batch.BatchSize,batch.TubesCount);

           
          

            // now assign the batch Id foreach BatchIngredient item
            foreach (var batchIngredientsItem in batchIngredients)
            {
               batchIngredientsItem.BatchId = insertedBatchId;
               _dataContext.BatchIngredients.Add(batchIngredientsItem);
                await _dataContext.SaveChangesAsync();
            }

            
            return insertedBatchId;

        }

        public   void Delete(int Id)
        {
          var originalEntity=     _dataContext.Batches.FirstOrDefault(x=>x.Id==Id);
          if (originalEntity!=null)
          {
            _dataContext.Batches.Remove(originalEntity);
              _dataContext.SaveChangesAsync();
          }
        }

        public async Task<Batch> Get(int Id)
        {
           //  return await _dataContext.Batches.FirstOrDefaultAsync(x=>x.Id==Id);

          // return await _dataContext.Batches.Include(x=>x.BatchIngredients)
          // .FirstOrDefaultAsync(x=>x.Id==Id);



          // modified to include product....
          // todo : enable lazy loading ....
          return await _dataContext.Batches.AsNoTracking().
          Include(x=>x.Product)
          .Include(x=>x.BatchIngredients).ThenInclude(x=>x.Ingredient)
          .AsNoTracking()
          .FirstOrDefaultAsync(x=>x.Id==Id);
        }


        // get without related entities....
         public async Task<Batch> GetBatch(int Id)
        { 
          return await _dataContext.Batches
          .FirstOrDefaultAsync(x=>x.Id==Id);
        }


        public async Task<IEnumerable<Batch>> GetAll()
        {

           // return await _dataContext.Batches.ToListAsync();
            return await _dataContext.Batches.Include(x=>x.Product).ToListAsync();
         }

        public async Task<Batch> Update(int Id,Batch batch)
        {
         var originalEntity=  await  _dataContext.Batches.FirstOrDefaultAsync(x=>x.Id==Id);
          if (originalEntity!=null)
          {
            // setting properties of original entity from parameter entity....
              originalEntity.Barcode = batch.Barcode;
              originalEntity.BatchNO = batch.BatchNO;
              originalEntity.BatchSize = batch.BatchSize;
            //  originalEntity.BatchStateId = batch.BatchStateId;
              originalEntity.CartoonPictureURL=batch.CartoonPictureURL;
             //   originalEntity.StartDate = batch.StartDate;
            //  originalEntity.EndDate = batch.EndDate;
              originalEntity.ExpDate = batch.ExpDate;
              originalEntity.MFgDate = batch.MFgDate;
              originalEntity.MFNO = batch.MFNO;
              originalEntity.NDCNO = batch.NDCNO;
              originalEntity.ProductId = batch.ProductId;
              originalEntity.Revision  = batch.Revision;
              originalEntity.RevisionDate = batch.RevisionDate;
             
              originalEntity.TubePictureURL = batch.TubePictureURL;
              originalEntity.UserId = batch.UserId;
              originalEntity.TubesCount = batch.TubesCount;
              originalEntity.TubeWeight = batch.TubeWeight;

                // set the state tomodified....
               _dataContext.Entry(originalEntity).Property(x=>x.Barcode).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.BatchNO).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.BatchSize).IsModified=true;
             
               _dataContext.Entry(originalEntity).Property(x=>x.CartoonPictureURL).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.ExpDate).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.MFgDate).IsModified=true;

               _dataContext.Entry(originalEntity).Property(x=>x.MFNO).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.NDCNO).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.ProductId).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.Revision).IsModified=true;
              _dataContext.Entry(originalEntity).Property(x=>x.RevisionDate).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.UserId).IsModified=true;


               _dataContext.Entry(originalEntity).Property(x=>x.TubesCount).IsModified=true;
               _dataContext.Entry(originalEntity).Property(x=>x.TubeWeight).IsModified=true;

                          
              await _dataContext.SaveChangesAsync();

              // Recreate the ingredients....one way of doing this ...
              // the other way is to compare the original entities with the new ones --
              // -- and make delete - update - insert statemetns based on the comparison

               var batchIngredients = this.generate_Batch_ingredient(batch.ProductId,batch.BatchSize,batch.TubesCount);

               // remove the old ingredients entries ....
               // get the original entities first....

               var originalIngredients = _dataContext.BatchIngredients.Where(x=>x.BatchId==originalEntity.Id);
               if (originalIngredients!=null)
               { _dataContext.BatchIngredients.RemoveRange(originalIngredients);}
               


            // now assign the batch Id foreach BatchIngredient item
            foreach (var batchIngredientsItem in batchIngredients)
            {
               batchIngredientsItem.BatchId = originalEntity.Id;
               _dataContext.BatchIngredients.Add(batchIngredientsItem);
                await _dataContext.SaveChangesAsync();
            }
             return originalEntity;
          }
          return null;
        }

        

        private List<BatchIngredient> generate_Batch_ingredient(int productId,decimal batchSize,int tubesCount)

        {
            List<BatchIngredient> batchIngredients=new List<BatchIngredient> ();
           // get product ingredients ....
             var productIngredients = _dataContext.ProductIngredients.Where(x=>x.ProductId==productId).ToList();
             // calculate the batch tube weight
             decimal BatchTubeSize = batchSize / tubesCount; // handle exceptions later...


           
             // now loop  through product ingredients 
             foreach (var productIngredientsItem  in productIngredients)
             {
                 BatchIngredient batchIngredient=new  BatchIngredient();
                 batchIngredient.BatchId = 0; // change to added batch Id ...
                 batchIngredient.IngredientId = productIngredientsItem.IngredientId;
                 batchIngredient.QTYPerTube = productIngredientsItem.Percentage;
                 batchIngredient.QTYPerBatch = BatchTubeSize * (batchIngredient.QTYPerTube/100m);
                 batchIngredients.Add(batchIngredient);
             }

             return batchIngredients;
        }


        public async Task <bool> SetBatchAsStarted(int Id)
        {
           var originalBatch = await _dataContext.Batches
          .FirstOrDefaultAsync(x=>x.Id==Id);

           if (originalBatch==null)
           {
            return false;
           }
           originalBatch.StartDate = DateTime.Now;
           originalBatch.BatchStateId =(int) BatchStatesEnum.processing;
           _dataContext.Entry(originalBatch).Property(x=>x.StartDate).IsModified=true;
            _dataContext.Entry(originalBatch).Property(x=>x.BatchStateId).IsModified=true;

           await _dataContext.SaveChangesAsync();
             return true;
          

        }


         
    }
}