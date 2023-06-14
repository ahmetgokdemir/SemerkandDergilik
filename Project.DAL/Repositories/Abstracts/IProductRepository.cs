using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> GetActivesProductsByCategory_of_FoodIDAsync(int Category_of_Food_id);
        IQueryable<Product> GetProductByIdwithCategory_of_FoodValueAsync(int product_id);
    }

}
