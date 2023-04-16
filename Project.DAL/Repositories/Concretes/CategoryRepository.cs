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
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(TechnosoftProjectContext context) : base(context)
        {

        }

        public IQueryable<string> GetActivesCategoryNamesAsync()
        {
            return _context.Set<Category>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).Select(x=> x.CategoryName);
        }

        public IQueryable<string> GetCategoryNameAccordingToProductAsync(int category_id)
        {
            IQueryable<string> categoryNameAccordingToProduct = _context.Set<Category>().Where(x => x.ID == category_id).Select(x => x.CategoryName);

            return categoryNameAccordingToProduct;
        }

    }
}
