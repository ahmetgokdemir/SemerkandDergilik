//using Project.DAL.Context;
//using Project.DAL.Repositories.Abstracts;
//using Project.ENTITIES.Enums;
//using Project.ENTITIES.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Project.DAL.Repositories.Concretes
//{
//    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
//    {
//        public MenuRepository(TechnosoftProjectContext context) : base(context)
//        {
//        }

//        public IQueryable<object> Get_ByUserID_Async_Repo(Guid userID)
//        {
//            var _entityFinal = _context.Set<Menu>().Where(x => x.AppUser.Id == userID && x.DataStatus != DataStatus.Deleted).AsQueryable();
        
//            return _entityFinal;
//        }

//    }
//}
