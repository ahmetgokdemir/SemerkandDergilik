using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Identity_Models;
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

        public async void Delete_OldFood_from_User(short foodID, AppUser _currentUser)
        {
            _iufjrep.Delete_OldFood_from_User_Repo(foodID, _currentUser);

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

        public async Task<List<Food>> Get_ByAll_exceptUserID_Async(Guid userID)
        {
            List<Food> found_Items = await _iufjrep.Get_ByAll_exceptUserID_Async_Repo(userID);

            if (found_Items == null)
            {
                return null;
            }

            return found_Items;
        }     

        public async Task<string> Control_IsExisted_InMyListBefore_Async(AppUser _userInfo, short foodID)
        {
            bool found_Item = await _iufjrep.Control_IsExisted_InMyListBefore_Async_Repo(_userInfo, foodID);

            int result_Code;

            // existed
            if (found_Item == true)
            {
                result_Code =  await _iufjrep.Update_MyList_Async_Repo(_userInfo, foodID);
            }
            // not existed.. will add
            else
            {
                result_Code = await _iufjrep.Add_Foodtem_toMyList_Async_Repo(_userInfo, foodID);
            }

            string result_Message;

            if (result_Code == 1)
            {
                result_Message = "Başarılı";
            }
            else
            {
                result_Message = "Hata";
            }

            return result_Message;
        }
         
        public async Task<IEnumerable<object>> GetFoodDetails_of_Member_Async(AppUser _currentUser, short foodID)
        {
            var found_Item = await _iufjrep.GetFoodDetails_of_Member_Async_Repo(_currentUser,foodID);

            if (found_Item == null)
            {
                return null;
            }

            return found_Item;
        }

    }
}
