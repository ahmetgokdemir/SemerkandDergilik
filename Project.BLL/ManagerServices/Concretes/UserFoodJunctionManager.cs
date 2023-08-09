using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Concretes
{
    public class UserFoodJunctionManager : BaseManager<UserFoodJunction>, IUserFoodJunctionManager
    {
        IUserFoodJunctionRepository _iufjrep;

        public UserFoodJunctionManager(IRepository<UserFoodJunction> irep, IUserFoodJunctionRepository iufjrep) : base(irep)
        {
            _iufjrep = iufjrep;

        }
        public async Task<IEnumerable<object>> Get_ByUserID_Async(Guid userID)
        {
            var found_Item = await _iufjrep.Get_ByUserID_Async_Repo(userID).ToListAsync();

            if (found_Item == null)
            {
                return null;
            }

            return found_Item;
        }
    }
}
