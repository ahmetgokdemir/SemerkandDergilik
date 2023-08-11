using Microsoft.EntityFrameworkCore;
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

        public async Task<List<CategoryofFood>> Get_ByAll_exceptUserID_Async_Repo(Guid userID)
        {
            // x.AppUser.Id == userID && x.DataStatus == ENTITIES.Enums.DataStatus.Deleted : önceden ekleyip sildiklerim
            List<CategoryofFood> allList_notexist = new List<CategoryofFood>();

            List<UserCategoryJunction> mydeletedList = new List<UserCategoryJunction>();


            mydeletedList = _context.Set<UserCategoryJunction>().Where(x => x.AppUser.Id == userID && x.DataStatus == ENTITIES.Enums.DataStatus.Deleted).ToList();

            //.Include(x => x.CategoryofFood)
            //.Include(x => x.AppUser)
            //.Select(x => //new

            //x.CategoryofFood.CategoryName_of_Foods).ToString()
            //CategoryofFood_Picture = x.CategoryofFood_Picture,
            //CategoryofFood_Status = x.CategoryofFood_Status,
            // AppUserId = x.AppUser.Id, // ID (IdentityUser'den gelir ve erişilemez onun yerine AppUser dan id e erişilir)
            // CategoryofFoodID = x.CategoryofFoodID


            List<CategoryofFood> mydeletedList2 = new List<CategoryofFood>();

            foreach (UserCategoryJunction not_exist in mydeletedList)
            {
                //mydeletedList2.Add( (CategoryofFood)_context.Set<CategoryofFood>().Where(x => x.ID == not_exist.CategoryofFoodID));

                CategoryofFood cof = _context.Set<CategoryofFood>().Where(x => x.ID == not_exist.CategoryofFoodID).FirstOrDefault();
                allList_notexist.Add(cof);
            }





            // diğer kullanıcıların listeleri (1)
            List<UserCategoryJunction> others = _context.Set<UserCategoryJunction>()
                .Where(x => (x.AppUser.Id != userID)).ToList();

            // (1) diğer kullanıclarda olup, userda da olanlar olabilir bunlar (2)de çıkarılmalı
            IQueryable<object> mines = _context.Set<UserCategoryJunction>()
                .Where(x=> x.AppUser.Id == userID && x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).AsQueryable();
            
            bool ignore_Item = false;

            // (2) çıkarma işlemi...
            foreach (short othersCategoryofFoodIDs in others.Select(x=> x.CategoryofFoodID))
            {
                foreach (UserCategoryJunction mine in mines)
                {
                    if (othersCategoryofFoodIDs == mine.CategoryofFoodID)
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
                    CategoryofFood cof2 = _context.Set<CategoryofFood>().Where(x => x.ID == othersCategoryofFoodIDs).FirstOrDefault();

                    if (cof2 != null)
                    {
                        // önceden diğer kullanıcılardan alıp da sildiği varsa user'ın tekrar listeye eklemesin
                        if (!allList_notexist.Contains(cof2))
                        {
                            allList_notexist.Add(cof2);
                        }
                    }
                    
                }

                ignore_Item = false;


            }

            return allList_notexist;
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

        public async void Delete_OldCategory_from_User_Repo(Guid accessibleID, UserCategoryJunction old_ucj)
        {
            /*ucj.DataStatus = ENTITIES.Enums.DataStatus.Updated;
            ucj.ModifiedDate = DateTime.Now;*/


            // T toBeUpdated = Find(entity.ID);

            // builder.HasKey(x => new { x.AccessibleID, x.CategoryofFoodID }); sayesinde 
            var toBeUpdated = _context.Set<UserCategoryJunction>().Find(accessibleID, old_ucj.CategoryofFoodID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;

            //if (toBeUpdated is CategoryofFood /* || entity is Food*/ )
            //{
            //    CategoryofFood c = toBeUpdated as CategoryofFood;


            //}

            _context.Entry(toBeUpdated).CurrentValues.SetValues(old_ucj);

            // _context.Save();
            _context.SaveChanges();

        }

        public async void Update_UserCategoryJuncTable_Repo(Guid accessibleID, short categoryofFood_ID, UserCategoryJunction ucj)
        {
            /*ucj.DataStatus = ENTITIES.Enums.DataStatus.Updated;
            ucj.ModifiedDate = DateTime.Now;*/


            // T toBeUpdated = Find(entity.ID);

            // builder.HasKey(x => new { x.AccessibleID, x.CategoryofFoodID }); sayesinde 
            var toBeUpdated = _context.Set<UserCategoryJunction>().Find(accessibleID, categoryofFood_ID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;

            //if (toBeUpdated is CategoryofFood /* || entity is Food*/ )
            //{
            //    CategoryofFood c = toBeUpdated as CategoryofFood;


            //}

            _context.Entry(toBeUpdated).CurrentValues.SetValues(ucj);

            // _context.Save();
            _context.SaveChanges();

        }

        public async Task<bool> Control_IsExisted_InMyListBefore_Async_Repo(Guid userID, short categoryID)
        {
            // var entity = _context.Set<T>().Find(id).AsQueryable(); --> find one item not for list
            // return Where(x => x.AppUser.Id == id).AsQueryable(); // AppUser.ID (erişilemiyor) --> short 

            // await --> _context.Set kullanılmaz !!!

            bool control_existing = _context.Set<UserCategoryJunction>()
                .Any(x => x.AppUser.Id == userID && x.CategoryofFoodID == categoryID && x.DataStatus == ENTITIES.Enums.DataStatus.Deleted);

            return control_existing;
        }

        public async Task<string> Update_MyList_Async_Repo(Guid accessibleID, short categoryID)
        {
            UserCategoryJunction ucj;
            var toBeUpdated = _context.Set<UserCategoryJunction>().Find(accessibleID, categoryID);
            ucj = toBeUpdated;
            ucj.CategoryofFood_Status = ENTITIES.Enums.ExistentStatus.Aktif;
            // ucj.CategoryofFood_Description = 
            ucj.ModifiedDate = DateTime.Now;


            ucj.DataStatus = ENTITIES.Enums.DataStatus.Updated; // ***

            _context.Entry(toBeUpdated).CurrentValues.SetValues(ucj);

            int success =  _context.SaveChanges();

            string result_Message;

            if (success == 1)
            {
                 result_Message = "Başarılı";
            }
            else
            {
                result_Message = "Hata";
            }

            return result_Message;
        }

        public async Task<string> Add_CategoryItem_toMyList_Async_Repo(Guid accessibleID, short categoryID, AppUser _userInfo)
        {
            UserCategoryJunction ucj = new UserCategoryJunction();
            ucj.AppUser = _userInfo;           

            ucj.CategoryofFoodID = categoryID;
            ucj.CategoryofFood_Status = ENTITIES.Enums.ExistentStatus.Aktif;
            // ucj.CategoryofFood_Description = 
            // ucj.CategoryofFood_Picture
            ucj.AccessibleID = accessibleID;


            /*
                EntityBase.cs
                
                public EntityBase()
                {
                    CreatedDate = DateTime.Now;
                    DataStatus = Enums.DataStatus.Inserted;
                }            
             
             */
            // ucj.CreatedDate = DateTime.Now;
            // ucj.DataStatus = ENTITIES.Enums.DataStatus.Inserted; // ***           

            // _context.Set<UserCategoryJunction>().AddAsync(ucj);
            _context.AddAsync(ucj);

            int success = _context.SaveChanges();

            string result_Message;

            if (success == 1)
            {
                result_Message = "Başarılı";
            }
            else
            {
                result_Message = "Hata";
            }

            return result_Message;
        }

    }
}
