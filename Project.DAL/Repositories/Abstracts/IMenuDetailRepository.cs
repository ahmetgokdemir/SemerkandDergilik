using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.MenuDetailRepository;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IMenuDetailRepository : IRepository<MenuDetail>
    {
        IQueryable<object> Get_FoodsofMenu_Async(short Menu_ID);
        IQueryable<CategoriesOfMenu_Repo> Get_CategoriesofMenu_Async(short Menu_ID);
        Task<bool> IsExist_FoodinMenu_Repo_Async(short selected_foodID, short menu_ID);
        void Insert_FoodonMenu_Repo_Async(short selected_foodID, string category_Name, short menu_ID);


    }
}
