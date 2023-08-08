using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Identity_Models;
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
        Task<List<CategoryofFood>> Get_ByAll_exceptUserID_Async(Guid userID);
        Task<string> Control_IsExisted_InMyListBefore_Async(Guid userID, short categoryID, AppUser _userInfo);

    }
}
