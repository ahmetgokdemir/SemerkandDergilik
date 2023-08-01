﻿using Microsoft.EntityFrameworkCore;
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

        public IQueryable<object> Get_ByUserID_Async(Guid userID)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list

            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 

            return _context.Set<UserCategoryJunction>()
                .Where(x => x.AppUser.Id == userID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted)
                .Include(x => x.CategoryofFood)
                .Include(x=>x.AppUser)
                .Select(x => new 
                {
                CategoryName_of_Foods = x.CategoryofFood.CategoryName_of_Foods,
                CategoryofFood_Picture = x.CategoryofFood_Picture,
                CategoryofFood_Status = x.CategoryofFood_Status,
                AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id e erişilir)
                CategoryofFoodID = x.CategoryofFoodID


                }
            ).AsQueryable();

            // return control_deneme;
        }

        public async Task<IEnumerable<object>> Get_ByUserID_with_CategoryID_Async(Guid userID, short categoryID)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list
            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 

            // await --> _context.Set kullanılmaz !!!

            IEnumerable<object> control_deneme =  _context.Set<UserCategoryJunction>()
                .Where(x => x.AppUser.Id == userID && x.CategoryofFoodID == categoryID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted)
                .Include(x => x.CategoryofFood)
                .Include(x => x.AppUser)
                .Select(x => new
                {
                    CategoryName_of_Foods = x.CategoryofFood.CategoryName_of_Foods, // include
                    CategoryofFood_Picture = x.CategoryofFood_Picture,
                    CategoryofFood_Status = x.CategoryofFood_Status,
                    AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id e erişilir)
                    CategoryofFoodID = x.CategoryofFoodID


                }
            ).ToList();

            return control_deneme;
        }
    }
}
