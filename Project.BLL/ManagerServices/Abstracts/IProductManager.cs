using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IProductManager : IManager<Product>
    {
        Task<IEnumerable<Product>> GetActivesProductsByCategory_of_FoodIDAsync(int Category_of_Food_id);
        Task<Product> GetProductByIdwithCategory_of_FoodValueAsync(int product_id);
    }

}
