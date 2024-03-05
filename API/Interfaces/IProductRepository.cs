using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();

       Task<Product> Get(int Id);
       
       Task<Product> GetWithIngredients(int Id);
       Task<int> Add (Product product);
        Task<bool> Add_Product_Dataset(Product product);
        Task<bool> Update(int Id,Product product);

        Task<bool> Update_Product_Dataset(int Id,Product product);
        void Delete(int Id);

    }
}