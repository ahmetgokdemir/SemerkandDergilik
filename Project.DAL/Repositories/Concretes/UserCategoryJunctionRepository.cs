using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.FoodRepository;

namespace Project.DAL.Repositories.Concretes
{
    public class UserCategoryJunctionRepository : BaseRepository<UserCategoryJunction>, IUserCategoryJunctionRepository
    {
        public UserCategoryJunctionRepository(TechnosoftProjectContext context) : base(context)
        {

        }


        public IQueryable<object> Get_ByGuidId_Async(Guid id)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list

            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 
            
            return _context.Set<UserCategoryJunction>().Where(x => x.AppUser.Id == id && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).Include(x => x.CategoryofFood).Select(x => new 
            {
                CategoryName_of_Foods = x.CategoryofFood.CategoryName_of_Foods,
                CategoryofFood_Picture = x.CategoryofFood_Picture,
                CategoryofFood_Status = x.CategoryofFood_Status


            }).AsQueryable();   
        }
    }
}
