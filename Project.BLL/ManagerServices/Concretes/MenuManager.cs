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
    public class MenuManager : BaseManager<Menu>, IMenuManager
    {
        //public MenuManager(IRepository<Menu> irep) : base(irep)
        //{
        //}

        IMenuRepository _mrep;

        public MenuManager(IRepository<Menu> irep, IMenuRepository mrep) : base(irep)
        {
            _mrep = mrep;
        }

        public async Task<IEnumerable<object>> Get_ByUserID_Async(Guid userID)
        {
            var found_Item = await _mrep.Get_ByUserID_Async_Repo(userID).ToListAsync();

            if (found_Item == null) {

                return null;
            }

            //FoodVM cvm = new FoodVM
            //{
            //    UserFoodJunctionDTOs = UserFoodJunctionList.Adapt<IEnumerable<UserFoodJunctionDTO>>().ToList(),
            //    JavascriptToRun = JSpopupPage
            //};

            return found_Item;
        }
    }
}
