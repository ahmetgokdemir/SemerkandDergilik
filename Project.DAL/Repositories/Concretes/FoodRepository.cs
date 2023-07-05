using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.DAL.Repositories.Concretes.MenuDetailRepository;

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
        public IQueryable<Food> GetActivesFoodsByCategory_of_FoodIDAsync(int Category_of_Food_id)
        {
            return _context.Set<Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.Category_of_FoodID == Category_of_Food_id).AsQueryable();

            //.Include(x=> x.Category_of_Food).AsQueryable();
        }

        // Kategoriye göre Ürünlerin isimleri..  
        public IQueryable<FoodDto_Repo> GetActivesFoodNamesByCategory_of_FoodIDAsync(int Category_of_Food_id)
        {

            return _context.Set<Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.Category_of_FoodID == Category_of_Food_id).Select(x => new FoodDto_Repo
            {
                ID = x.ID,
                FoodName =  x.FoodName
                
            }).AsQueryable();   // ToList


            //.Include(x=> x.Category_of_Food).AsQueryable();
        }

        // Kullanılmadı !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // Ürünü, kategori bilgileri ile getirmek...
        public IQueryable<Food> GetFoodByIdwithCategory_of_FoodValueAsync(int Food_id)
        {
            return _context.Set<Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.ID == Food_id).Include(x => x.Category_of_Food).AsQueryable();
        }

    }
}
