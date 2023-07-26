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
        Task<IEnumerable<Food>> Get_FoodsByCategoryID_Async(int categoryid);
        Task<Food> GetFoodByIdwithCategoryofFoodValueAsync(int Food_id);
        //Task<IEnumerable<FoodDto_Repo>> GetActivesFoodNamesByCategoryofFoodIDAsync(int CategoryofFood_id);

    }

}
