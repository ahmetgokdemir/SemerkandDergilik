using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IUserFoodJunctionManager : IManager<UserFoodJunction>
    {
        Task<IEnumerable<object>> Get_ByUserID_Async(Guid userID);            
                
        Task<IEnumerable<object>> Get_ByUserID_with_FoodID_Async(Guid userID, short foodID);

        void Delete_OldFood_from_User(Guid accessibleID, UserFoodJunction passive_UserFoodJunction);

        void Update_UserFoodJuncTable(Guid accessibleID, short food_ID, UserFoodJunction ufj);

        Task<List<Food>> Get_ByAll_exceptUserID_Async(Guid userID);
        Task<string> Control_IsExisted_InMyListBefore_Async( AppUser _userInfo, short foodID);

    }
}
