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
        Task<IEnumerable<object>> Get_ByUserID_Async(Guid userID);
        Task<IEnumerable<object>> Get_ByUserID_with_CategoryID_Async(Guid userID, short categoryID );
        void Delete_OldCategory_from_User(Guid accessibleID, short old_categoryID, UserCategoryJunction old_ucj);
        void Update_UserCategoryJuncTable(Guid accessibleID, short categoryofFood_ID, UserCategoryJunction ucj);

        // Delete_OldCategory_from_User
    }
}
