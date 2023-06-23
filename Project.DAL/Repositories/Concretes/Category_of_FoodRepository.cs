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
    public class Category_of_FoodRepository : BaseRepository<Category_of_Food>, ICategory_of_FoodRepository
    {
        public Category_of_FoodRepository(TechnosoftProjectContext context) : base(context)
        {

        }

        public IQueryable<string> GetActivesCategory_of_FoodNamesAsync()
        {
            return _context.Set<Category_of_Food>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).Select(x=> x.Category_of_FoodName);
        }

        // kullanılmadı
        public IQueryable<string> GetCategory_of_FoodNameAccordingToFoodAsync(int Category_of_Food_id)
        {
            IQueryable<string> Category_of_FoodNameAccordingToFood = _context.Set<Category_of_Food>().Where(x => x.ID == Category_of_Food_id).Select(x => x.Category_of_FoodName);

            return Category_of_FoodNameAccordingToFood;
        }

    }
}
