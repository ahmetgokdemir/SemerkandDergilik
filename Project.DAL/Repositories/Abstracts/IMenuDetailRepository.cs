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
        IQueryable<MenuDetail_Repo> Get_FoodsofMenu_Async(int Menu_ID);
        IQueryable<CategoriesOfMenu_Repo> Get_CategoriesofMenu_Async(int Menu_ID);

    }
}
