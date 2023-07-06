using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.MenuDetailRepository;

namespace Project.BLL.ManagerServices.Concretes
{
    public class MenuDetailManager : BaseManager<MenuDetail>, IMenuDetailManager
    {
        IMenuDetailRepository _mdrep;

        public MenuDetailManager(IRepository<MenuDetail> irep, IMenuDetailRepository mdrep) : base(irep)
        {
            _mdrep = mdrep;
        }

        public async Task<IEnumerable<object>> Get_FoodsofMenu_Async(int Menu_ID)
        {            
            var Foods = await _mdrep.Get_FoodsofMenu_Async(Menu_ID).ToListAsync(); 
            // convert ıqueryable to IEnumerable (using namespace EntityFrameworkCore)

            return Foods;
        }

         public async Task<IEnumerable<CategoriesOfMenu_Repo>> Get_CategoriesofMenu_Async(int Menu_ID)
        {

            var Categories = await _mdrep.Get_CategoriesofMenu_Async(Menu_ID).ToListAsync();
            // convert ıqueryable to IEnumerable (using namespace EntityFrameworkCore)


            return Categories;
        }
    }
}
