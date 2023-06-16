using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface ICategory_of_FoodRepository : IRepository<Category_of_Food>
    {
        IQueryable<string> GetActivesCategory_of_FoodNamesAsync();
        IQueryable<string> GetCategory_of_FoodNameAccordingToFoodAsync(int Category_of_Food_id);
         
    }
}
