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
        IQueryable<Food> GetActivesFoodsByCategory_of_FoodIDAsync(int Category_of_Food_id);
        IQueryable<Food> GetFoodByIdwithCategory_of_FoodValueAsync(int Food_id);
        Task<IQueryable<FoodDto_Repo>> GetActivesFoodNamesByCategory_of_FoodIDAsync(int Category_of_Food_id);

    }

}
