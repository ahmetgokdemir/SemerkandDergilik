using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IUserCategoryJunctionRepository : IRepository<UserCategoryJunction>
    {
        IQueryable<object> Get_ByUserID_Async(Guid userID);

        Task<IEnumerable<object>> Get_ByUserID_with_CategoryID_Async(Guid userID, short categoryID);
          
    }
}
