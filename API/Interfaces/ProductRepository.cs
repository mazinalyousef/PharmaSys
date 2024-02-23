using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace API.Interfaces
{
    public class ProductRepository : IProductRepository
    {

        private readonly DataContext _dataContext;
      
        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Task<int> Add(Product batch)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Add_Product_Dataset(Product product)
        {
            bool committed=false;
            int insertedId=0;
            using (IDbContextTransaction transaction=await _dataContext.Database.BeginTransactionAsync() )
            {
                    try
                    {
                      // add product 

                    var insertedProductEntity= _dataContext.Products.Add(product);
                    // insert new product ingredients
                    /*
                    if (product.ProductIngredients!=null)
                    {
                      foreach(var newItem in product.ProductIngredients)
                    {
                        ProductIngredient productIngredient =new ProductIngredient ()
                        {
                            ProductId = insertedId,
                            IngredientId= newItem.IngredientId,
                            Percentage = newItem.Percentage
                        };
                        _dataContext.ProductIngredients.Add(productIngredient);
                        await _dataContext.SaveChangesAsync();
                       
                    }
                    }
                    */
                    await _dataContext.SaveChangesAsync();
                     insertedId = insertedProductEntity.Entity.Id;
                    
                    
                    await transaction.CommitAsync();
                    committed=true;
                    }
                    catch(Exception ex)
                    {
                     var Error =ex.ToString();
                     await transaction.RollbackAsync();
                    }
                    return committed;    
            }
        }

        public void Delete(int Id)
        {
            var originalEntity= _dataContext.Products.FirstOrDefault(x=>x.Id==Id);
            if (originalEntity!=null)
            {
              _dataContext.Products.Remove(originalEntity);
              _dataContext.SaveChanges();
            }
        }

        public async Task<Product> Get(int Id)
        {
              return await _dataContext.Products.Where(x=>x.Id==Id) 
             .FirstOrDefaultAsync();
        }

         public async Task<Product> GetWithIngredients(int Id)
        {
              return await _dataContext.Products.Include(x=>x.ProductIngredients).
              Where(x=>x.Id==Id) 
             .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Product>> GetAll()
        {
           return await _dataContext.Products.ToListAsync();
        }

        public Task<bool> Update(int Id, Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update_Product_Dataset(int Id, Product product)
        {

            bool committed=false;
            using (IDbContextTransaction transaction=await _dataContext.Database.BeginTransactionAsync() )
            {
                    try
                    {
                    // delete the original product ingredients
                    var originalIngredients =await _dataContext.ProductIngredients.Where(x=>x.ProductId==Id).ToListAsync();
                    _dataContext.ProductIngredients.RemoveRange(originalIngredients);
                    // update product
                     var originalProduct =await _dataContext.Products.FirstOrDefaultAsync(x=>x.Id==Id);
                      originalProduct.ProductName = product.ProductName;
                      originalProduct.ProductTypeId = product.ProductTypeId;
                     _dataContext.Entry(originalProduct).Property(x=>x.ProductName).IsModified=true;
                     _dataContext.Entry(originalProduct).Property(x=>x.ProductTypeId).IsModified=true;
                      await  _dataContext.SaveChangesAsync();
                    // insert new product ingredients
                    if (product.ProductIngredients!=null)
                    {foreach(var newItem in product.ProductIngredients)
                    {
                        ProductIngredient productIngredient =new ProductIngredient ()
                        {
                            ProductId = newItem.ProductId,
                            IngredientId= newItem.IngredientId,
                            Percentage = newItem.Percentage
                        };
                        _dataContext.ProductIngredients.Add(productIngredient);
                        await _dataContext.SaveChangesAsync();
                       
                    }}
                    

                    await transaction.CommitAsync();
                    committed=true;
                    }
                    catch(Exception ex )
                    {
                        var err =ex.ToString();
                     await transaction.RollbackAsync();
                    }
                    return committed;    
            }
        }
    }
}