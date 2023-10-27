using Project.ENTITIES.Identity_Models;
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
        // FoodList_forMember listelemede kullanılanlar...
        IQueryable<object> Get_ByUserID_Async_Repo(Guid userID);
        // FoodList_forMember listelemede kullanılanlar...


        // update güncelleme de kullanılanlar...
        Task<IEnumerable<object>> Get_ByUserID_with_FoodID_Async_Repo(Guid userID, short foodID);

        void Delete_OldFood_from_User_Repo(short foodID, AppUser _currentUser);

        void Update_UserFoodJuncTable_Repo(Guid accessibleID, short categoryofFood_ID, UserFoodJunction ufj);
        // update güncelleme de kullanılanlar...

        Task<List<Food>> Get_ByAll_exceptUserID_Async_Repo(Guid userID);   

        Task<bool> Control_IsExisted_InMyListBefore_Async_Repo(AppUser _userInfo, short foodID);

        Task<int> Update_MyList_Async_Repo(AppUser _userInfo, short foodID);

        Task<int> Add_Foodtem_toMyList_Async_Repo(AppUser _userInfo, short foodID);

        Task<IEnumerable<object>> GetFoodDetails_of_Member_Async_Repo(AppUser _userInfo, short foodID);



    }
}
