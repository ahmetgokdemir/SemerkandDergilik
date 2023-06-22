using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IMenuDetailRepository : IRepository<MenuDetail>
    {
        IQueryable<object> Get_FoodsofMenu_Async(int Menu_ID);
    }
}
