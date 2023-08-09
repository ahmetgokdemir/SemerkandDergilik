using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Concretes
{
    public class UserFoodJunctionRepository : BaseRepository<UserFoodJunction>, IUserFoodJunctionRepository
    {
        public UserFoodJunctionRepository(TechnosoftProjectContext context) : base(context)
        {

        }

        public IQueryable<object> Get_ByUserID_Async_Repo(Guid userID)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list

            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 

            return _context.Set<UserFoodJunction>()
                .Where(x => x.AppUser.Id == userID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted)
                .Include(x => x.Food)
                .Include(x => x.AppUser)
                .Select(x => new
                {
                    Food_Name = x.Food.Food_Name,
                    Food_Picture = x.Food_Picture,
                    Food_Status = x.Food_Status,
                    AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id e erişilir)
                    FoodID = x.FoodID


                }
            ).AsQueryable();

            // return control_deneme;
        }

    }
}
