using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IMenuDetailManager : IManager<MenuDetail>
    {
        Task<IEnumerable<object>> Get_FoodsofMenu_Async(int Menu_ID);
    }
}
