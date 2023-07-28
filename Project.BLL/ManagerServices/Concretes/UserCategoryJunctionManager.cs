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
    public class UserCategoryJunctionManager : BaseManager<UserCategoryJunction>, IUserCategoryJunctionManager
    {
        IUserCategoryJunctionRepository _iucjrep; 

        public UserCategoryJunctionManager(IRepository<UserCategoryJunction> irep, IUserCategoryJunctionRepository iucjrep) : base(irep)
        {
            _iucjrep = iucjrep;
        }
    }
}
