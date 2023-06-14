using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface ICategory_of_FoodManager : IManager<Category_of_Food>
    {

        Task<IEnumerable<string>> GetActivesCategory_of_FoodNamesAsync();
        Task<string> GetCategory_of_FoodNameAccordingToProductAsync(int Category_of_Food_id);

    }
}
