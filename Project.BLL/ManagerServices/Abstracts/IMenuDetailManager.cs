using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.MenuDetailRepository;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IMenuDetailManager : IManager<MenuDetail>
    {
        Task<IEnumerable<MenuDetail_Repo>> Get_FoodsofMenu_Async(int Menu_ID);
        Task<IEnumerable<CategoriesOfMenu_Repo>> Get_CategoriesofMenu_Async(int Menu_ID);
        
    }
}
