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

        public async Task<IEnumerable<object>> Get_ByUserID_with_FoodID_Async(Guid userID, short foodID)
        {
            var getfoodItem_byUserID = await _iufjrep.Get_ByUserID_with_FoodID_Async_Repo(userID, foodID);

            if (getfoodItem_byUserID == null)
            {
                return null;
            }

            return getfoodItem_byUserID;
        }

        public async void Delete_OldFood_from_User(Guid accessibleID, UserFoodJunction passive_UserFoodJunction)
        {
            _iufjrep.Delete_OldFood_from_User_Repo(accessibleID, passive_UserFoodJunction);

            //if (found_Item == null)
            //{
            //    return null;
            //}

            //return found_Item;
        }

        public async void Update_UserFoodJuncTable(Guid accessibleID, short food_ID, UserFoodJunction ufj)
        {
            _iufjrep.Update_UserFoodJuncTable_Repo(accessibleID, food_ID, ufj);

        }

        /* 
         
                public async Task<List<CategoryofFood>> Get_ByAll_exceptUserID_Async(Guid userID)
        {
            List<CategoryofFood> found_Items = await _iucjrep.Get_ByAll_exceptUserID_Async_Repo(userID);

            if (found_Items == null)
            {
                return null;
            }

            return found_Items;
        }






        public async Task<string> Control_IsExisted_InMyListBefore_Async(Guid userID, short categoryID, AppUser _userInfo)
        {
            bool found_Item = await _iucjrep.Control_IsExisted_InMyListBefore_Async_Repo(userID, categoryID);

            // existed
            if (found_Item == true)
            {
                return await _iucjrep.Update_MyList_Async_Repo(_userInfo.AccessibleID, categoryID);
            }
            // not existed.. will add
            return await _iucjrep.Add_CategoryItem_toMyList_Async_Repo(_userInfo.AccessibleID, categoryID, _userInfo);
        }

          
         */
    }
}
