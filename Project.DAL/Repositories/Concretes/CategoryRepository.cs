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
        public CategoryRepository(SemerkandDergilikContext context) : base(context)
        {

        }

        public IQueryable<string> GetActivesCategoryNamesAsync()
        {
            return _context.Set<Category>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).Select(x=> x.CategoryName).AsQueryable();
        }

        public async Task<string> GetCategoryNameAccordingToProductAsync(int category_id)
        { 
           string deneme =   _context.Set<Category>().Where(x => x.ID == category_id).Select(x => x.CategoryName).ToString();

            return deneme;
        }

    }
}
