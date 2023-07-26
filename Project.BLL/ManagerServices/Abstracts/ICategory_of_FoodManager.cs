using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface ICategoryofFoodManager : IManager<CategoryofFood>
    {

        Task<IEnumerable<string>> GetActivesCategoryofFoodNamesAsync();
        Task<string> GetCategoryofFoodNameAccordingToFoodAsync(int CategoryofFood_id);

    }
}
