using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Concretes
{
    public class UserCategoryJunctionManager : BaseManager<UserCategoryJunction>, IUserCategoryJunctionManager
    {
        IUserCategoryJunctionRepository _iucjrep; 

        public UserCategoryJunctionManager(IRepository<UserCategoryJunction> irep, IUserCategoryJunctionRepository iucjrep) : base(irep)
        {
            _iucjrep = iucjrep;
        }
        public async Task<IEnumerable<object>> Get_ByGuidId_Async(Guid id)
        {
            var found_Item = await _iucjrep.Get_ByGuidId_Async(id).ToListAsync();

            if (found_Item == null)
            {
                return null;
            }

            return found_Item;
        }
    }
}
