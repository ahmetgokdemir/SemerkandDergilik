using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.FoodRepository;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IFoodManager : IManager<Food>
    {
        Task<IEnumerable<Food>> GetActivesFoodsByCategory_of_FoodIDAsync(int Category_of_Food_id);
        Task<Food> GetFoodByIdwithCategory_of_FoodValueAsync(int Food_id);
        Task<IEnumerable<FoodDto_Repo>> GetActivesFoodNamesByCategory_of_FoodIDAsync(int Category_of_Food_id);

    }

}
