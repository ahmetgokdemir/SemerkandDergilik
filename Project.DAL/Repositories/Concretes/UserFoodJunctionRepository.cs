using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Identity_Models;
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

            //var _entity = _context.Set<UserFoodJunction>()
            // .LeftJoin(_context.Set<ImageofFood>(),
            //     ufj => ufj.ID,
            //     iof => iof.UserFoodJunctionID,
            //     (ufj, iof) => new                  {


            //         ufj = ufj,
            //         iof = iof

            //     }).Where(a => a.iof.IsProfile == true);

 
                //var _entityFinal2 = _context.Set<UserFoodJunction>()
                //.Include(x => x.ImageofFoods).Include(x => x.Food)
                //.Select(x => new
                //{
                //    Food_Image = x.ImageofFoods.FirstOrDefault(y => y.IsProfile).Food_Image,
                //    Food_Name = x.Food.Food_Name,
                //}
                //).ToList();

                //  _context.Set<UserFoodJunction>() yerine _entity
                var _entityFinal = _context.Set<UserFoodJunction>()
                .Where(x => x.AppUser.Id == userID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted)
                .Include(x => x.Food)
                .Include(x => x.AppUser)
                .Include(x => x.ImageofFoods)
                //.//Join(x=> x.ImageofFoods)
                //.Join(x => x.ImageofFoods).Where(u => u.FoodID == u.);//Foreign key dene...
                .Select(x => new
                {
                    Food_Name = x.Food.Food_Name,
                    Food_Price = x.Food_Price,
                    Food_Status = x.Food_Status,
                    Food_Picture = x.ImageofFoods.FirstOrDefault(y => y.IsProfile).Food_Image,
                    //Food_Picture = x.ImageofFoods.FirstOrDefault(i => i.IsProfile).Food_Image,
                    Food_Description = x.Food_Description,
                    // AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id e erişilir)
                    FoodID = x.FoodID


                }
            ).AsQueryable();

            return _entityFinal;

             // entity.GroupJoin(_context.Set<ImageofFood>()).Where(u => u.FoodID == u.);
            ;
        }

        public async Task<IEnumerable<object>> Get_ByUserID_with_FoodID_Async_Repo(Guid userID, short foodID)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list
            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 

            // await --> _context.Set kullanılmaz !!!

            IEnumerable<object> getfoodItem_byUserID = _context.Set<UserFoodJunction>()
                .Where(x => x.AppUser.Id == userID && x.FoodID == foodID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted)
                .Include(x => x.Food)
                .Include(x => x.AppUser)
                .Select(x => new
                {
                    Food_Name = x.Food.Food_Name, // include
                    Food_Price = x.Food_Price,
                    Food_Status = x.Food_Status,
                    Food_Picture = x.Food_Picture,
                    Food_Description = x.Food_Description,
                    AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id'e erişilir)
                    FoodID = x.FoodID
                }
            ).ToList();

            return getfoodItem_byUserID;
        }

        public async void Delete_OldFood_from_User_Repo(Guid accessibleID, UserFoodJunction passive_UserFoodJunction)
        {
            // builder.HasKey(x => new { x.AccessibleID, x.CategoryofFoodID }); sayesinde 
            var toBeUpdated = _context.Set<UserFoodJunction>().Find(accessibleID, passive_UserFoodJunction.FoodID);


            // become passive 
            _context.Entry(toBeUpdated).CurrentValues.SetValues(passive_UserFoodJunction);

            // _context.Save();
            _context.SaveChanges();

        }

        public async void Update_UserFoodJuncTable_Repo(Guid accessibleID, short categoryofFood_ID, UserFoodJunction ufj)
        {
            /*ucj.DataStatus = ENTITIES.Enums.DataStatus.Updated;
            ucj.ModifiedDate = DateTime.Now;*/


            // T toBeUpdated = Find(entity.ID);

            // builder.HasKey(x => new { x.AccessibleID, x.CategoryofFoodID }); sayesinde 
            var toBeUpdated = _context.Set<UserFoodJunction>().Find(accessibleID, categoryofFood_ID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;

            //if (toBeUpdated is CategoryofFood /* || entity is Food*/ )
            //{
            //    CategoryofFood c = toBeUpdated as CategoryofFood;


            //}

            _context.Entry(toBeUpdated).CurrentValues.SetValues(ufj);

            // _context.Save();
            _context.SaveChanges();

        }


        public async Task<List<Food>> Get_ByAll_exceptUserID_Async_Repo(Guid userID)
        {
            // x.AppUser.Id == userID && x.DataStatus == ENTITIES.Enums.DataStatus.Deleted : önceden ekleyip sildiklerim
            List<Food> allList_notexist = new List<Food>();

            List<UserFoodJunction> mydeletedList = new List<UserFoodJunction>();


            mydeletedList = _context.Set<UserFoodJunction>().Where(x => x.AppUser.Id == userID && x.DataStatus == ENTITIES.Enums.DataStatus.Deleted).ToList();

            int control = 0;


            foreach (UserFoodJunction not_exist in mydeletedList)
            {
                Food fd = _context.Set<Food>().Where(x => x.ID == not_exist.FoodID).FirstOrDefault();
                allList_notexist.Add(fd);
            }

            // diğer kullanıcıların listeleri (1)
            List<UserFoodJunction> others = _context.Set<UserFoodJunction>()
                .Where(x => (x.AppUser.Id != userID)).ToList();

            // (1) diğer kullanıclarda olup, userda da olanlar olabilir bunlar (2)de çıkarılmalı
            IQueryable<object> mines = _context.Set<UserFoodJunction>()
                .Where(x => x.AppUser.Id == userID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).AsQueryable();

            bool ignore_Item = false;

            // (2) çıkarma işlemi...
            foreach (short othersCategoryofFoodIDs in others.Select(x => x.FoodID))
            {
                foreach (UserFoodJunction mine in mines)
                {
                    if (othersCategoryofFoodIDs == mine.FoodID)
                    {
                        // kullanıcıda da var demektir çık dönğüden
                        ignore_Item = true;
                        //break;
                    }
                    else
                    {
                        continue;
                    }


                }
                if (ignore_Item == false)
                {
                    Food fd2 = _context.Set<Food>().Where(x => x.ID == othersCategoryofFoodIDs).FirstOrDefault();

                    if (fd2 != null)
                    {
                        // önceden diğer kullanıcılardan alıp da sildiği varsa user'ın tekrar listeye eklemesin
                        if (!allList_notexist.Contains(fd2))
                        {
                            allList_notexist.Add(fd2);
                        }
                    }

                }

                ignore_Item = false;


            }

            return allList_notexist;
        }

        public async Task<bool> Control_IsExisted_InMyListBefore_Async_Repo(AppUser _userInfo, short foodID)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list
            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 

            // await --> _context.Set kullanılmaz !!!

            bool control_existing = _context.Set<UserFoodJunction>()
                .Any(x => x.AppUser.Id == _userInfo.Id && x.FoodID == foodID && x.DataStatus == ENTITIES.Enums.DataStatus.Deleted);

            return control_existing;
        }

        public async Task<int> Update_MyList_Async_Repo(AppUser _userInfo, short foodID)
        {
            UserFoodJunction ufj;

            var toBeUpdated = _context.Set<UserFoodJunction>().Find(_userInfo.AccessibleID, foodID);

            ufj = toBeUpdated;

            ufj.Food_Status = ENTITIES.Enums.ExistentStatus.Aktif;
            // ucj.CategoryofFood_Description = 
            ufj.ModifiedDate = DateTime.Now;


            ufj.DataStatus = ENTITIES.Enums.DataStatus.Updated; // ***

            _context.Entry(toBeUpdated).CurrentValues.SetValues(ufj);

            return _context.SaveChanges();

        }

        public async Task<int> Add_CategoryItem_toMyList_Async_Repo(AppUser _userInfo, short foodID)
        {
            UserFoodJunction ufj = new UserFoodJunction();
            ufj.AppUser = _userInfo;

            ufj.FoodID = foodID;
            ufj.Food_Status = ENTITIES.Enums.ExistentStatus.Aktif;
            // ucj.CategoryofFood_Description = 
            // ucj.CategoryofFood_Picture
            ufj.AccessibleID = _userInfo.AccessibleID;


            _context.AddAsync(ufj);

            //int result = _context.SaveChanges();
            return _context.SaveChanges();


        }

    }
}
