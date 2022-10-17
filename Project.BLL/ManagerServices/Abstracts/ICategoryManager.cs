using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface ICategoryManager : IManager<Category>
    {

        Task<IEnumerable<string>> GetActivesCategoryNamesAsync();
        Task<string> GetCategoryNameAccordingToProductAsync(int category_id);

    }
}
