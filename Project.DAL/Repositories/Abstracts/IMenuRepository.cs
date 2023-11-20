using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IMenuRepository : IRepository<Menu>
    {
        IQueryable<object> Get_ByUserID_Async_Repo(Guid userID);
    }
}
