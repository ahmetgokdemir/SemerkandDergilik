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
        IQueryable<Food> Get_FoodsByCategoryID_Async(int categoryid);
        IQueryable<Food> GetFoodByIdwithCategoryofFoodValueAsync(int Food_id);
        // IQueryable<FoodDto_Repo> GetActivesFoodNamesByCategoryofFoodIDAsync(int CategoryofFood_id);

        /*
         
            IQueryable<string> GetActivesCategoryofFoodNamesAsync();

            IQueryable<string> GetCategoryofFoodNameAccordingToFoodAsync(short CategoryofFood_id);
         
         */

    }

}
