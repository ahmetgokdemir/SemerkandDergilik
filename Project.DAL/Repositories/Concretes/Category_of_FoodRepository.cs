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
    public class CategoryofFoodRepository : BaseRepository<CategoryofFood>, ICategoryofFoodRepository
    {
        public CategoryofFoodRepository(TechnosoftProjectContext context) : base(context)
        {

        }

        public IQueryable<string> GetActivesCategoryofFoodNamesAsync()
        {
            return _context.Set<CategoryofFood>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).Select(x=> x.CategoryName_of_Foods);
        }

        // kullanılmadı
        public IQueryable<string> GetCategoryofFoodNameAccordingToFoodAsync(int CategoryofFood_id)
        {
            IQueryable<string> CategoryofFoodNameAccordingToFood = _context.Set<CategoryofFood>().Where(x => x.ID == CategoryofFood_id).Select(x => x.CategoryName_of_Foods);

            return CategoryofFoodNameAccordingToFood;
        }

    }
}
