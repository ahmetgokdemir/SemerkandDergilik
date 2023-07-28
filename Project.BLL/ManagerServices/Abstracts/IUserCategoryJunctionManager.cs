using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IUserCategoryJunctionManager : IManager<UserCategoryJunction>
    {
        Task<IEnumerable<object>> Get_ByGuidId_Async(Guid id);
    }
}
