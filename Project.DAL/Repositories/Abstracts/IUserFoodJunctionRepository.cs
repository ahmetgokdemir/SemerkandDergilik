using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IUserFoodJunctionRepository : IRepository<UserFoodJunction>
    {
        IQueryable<object> Get_ByUserID_Async_Repo(Guid userID);

        /*
          
         Task<IEnumerable<object>> Get_ByUserID_with_CategoryID_Async(Guid userID, short categoryID);
                  
        void Delete_OldCategory_from_User_Repo(Guid accessibleID, short old_categoryID, UserCategoryJunction old_ucj);

        void Update_UserCategoryJuncTable_Repo(Guid accessibleID, short categoryofFood_ID, UserCategoryJunction ucj);

        Task<List<CategoryofFood>> Get_ByAll_exceptUserID_Async_Repo(Guid userID);

        Task<bool> Control_IsExisted_InMyListBefore_Async_Repo(Guid userID, short categoryID);

        Task<string> Update_MyList_Async_Repo(Guid accessibleID, short categoryID);

        Task<string> Add_CategoryItem_toMyList_Async_Repo(Guid accessibleID, short categoryID, AppUser _userInfo);

          
         */
    }
}
