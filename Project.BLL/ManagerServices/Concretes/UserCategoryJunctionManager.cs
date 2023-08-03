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
        public async Task<IEnumerable<object>> Get_ByUserID_Async(Guid userID)
        {
            var found_Item = await _iucjrep.Get_ByUserID_Async(userID).ToListAsync();

            if (found_Item == null)
            {
                return null;
            }

            return found_Item;
        }

        public async Task<IEnumerable<object>> Get_ByUserID_with_CategoryID_Async(Guid userID, short categoryID)
        {
            var found_Item = await _iucjrep.Get_ByUserID_with_CategoryID_Async(userID, categoryID);

            if (found_Item == null)
            {
                return null;
            }

            return found_Item;
        }

        
        public async void Delete_OldCategory_from_User(Guid accessibleID, short old_categoryID, UserCategoryJunction old_ucj)
        {
            _iucjrep.Delete_OldCategory_from_User_Repo(accessibleID, old_categoryID, old_ucj);

            //if (found_Item == null)
            //{
            //    return null;
            //}

            //return found_Item;
        }

        public async void Update_UserCategoryJuncTable(Guid accessibleID, short categoryofFood_ID, UserCategoryJunction ucj)
        {
            _iucjrep.Update_UserCategoryJuncTable_Repo(accessibleID, categoryofFood_ID, ucj);


        }


    }
}
