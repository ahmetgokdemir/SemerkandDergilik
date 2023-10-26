using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Project.DAL.Repositories.Concretes.MenuDetailRepository;

namespace Project.DAL.Repositories.Concretes
{
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        public class FoodDto_Repo
        {
            public int ID { get; set; } // Aktif, Pasif            
            public string FoodName { get; set; }
        }


        public FoodRepository(TechnosoftProjectContext context) : base(context)
        {

        }

        // Kategoriye göre Ürünler.. Include
        public IQueryable<Food> Get_FoodsByCategoryID_Async(int categoryid)
        {
            // return _context.Set<Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.CategoryofFoodID == categoryid).AsQueryable();
            // *** İsmail Bey - Değişiklikleri
            return null;

            //.Include(x=> x.CategoryofFood).AsQueryable();
        }

        /*
        // Kategoriye göre Ürünlerin isimleri..  
        public IQueryable<FoodDto_Repo> GetActivesFoodNamesByCategoryofFoodIDAsync(int CategoryofFood_id)
        {

            return _context.Set<Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.CategoryofFoodID == CategoryofFood_id).Select(x => new FoodDto_Repo
            {
                ID = x.ID,
                FoodName =  x.FoodName
                
            }).AsQueryable();   // ToList


            //.Include(x=> x.CategoryofFood).AsQueryable();
        }
        */

        // Kullanılmadı !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Ürünü, kategori bilgileri ile getirmek...
        public IQueryable<Food> GetFoodByIdwithCategoryofFoodValueAsync(int Food_id)
        {
            //return _context.Set<Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.ID == Food_id).Include(x => x.CategoryofFood).AsQueryable();
            // *** İsmail Bey - Değişiklikleri
            return null;
        
        }



        //public IQueryable<string> GetActivesCategoryofFoodNamesAsync()
        //{
        //    return _context.Set<CategoryofFood>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).Select(x => x.CategoryName_of_Foods);
        //}

        //// kullanılmadı
        //public IQueryable<string> GetCategoryofFoodNameAccordingToFoodAsync(short CategoryofFood_id)
        //{
        //    IQueryable<string> CategoryofFoodNameAccordingToFood = _context.Set<CategoryofFood>().Where(x => x.ID == CategoryofFood_id).Select(x => x.CategoryName_of_Foods);

        //    return CategoryofFoodNameAccordingToFood;
        //}

    }
}
