using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.FoodRepository;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IFoodRepository : IRepository<Food>
    {
        IQueryable<Food> GetFoodsByCategoryID_Async(int categoryid);
        IQueryable<Food> GetFoodByIdwithCategory_of_FoodValueAsync(int Food_id);
        // IQueryable<FoodDto_Repo> GetActivesFoodNamesByCategory_of_FoodIDAsync(int Category_of_Food_id);

    }

}
