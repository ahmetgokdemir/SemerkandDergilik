using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Concretes
{
    public class CategoryManager : BaseManager<Category>, ICategoryManager
    {
        ICategoryRepository _crep;

        public CategoryManager(IRepository<Category> irep, ICategoryRepository crep) : base(irep)
        {
            _crep = crep;
        }


        public async Task<IEnumerable<string>> GetActivesCategoryNamesAsync()
        {
            var categoryNames = await _crep.GetActivesCategoryNamesAsync().ToListAsync(); // convert ıqueryable to IEnumerable

            return categoryNames;
        }

        public async Task<string> GetCategoryNameAccordingToProductAsync(int category_id)
        {
            string deneme = _crep.GetCategoryNameAccordingToProductAsync(category_id).ToString(); // convert ıqueryable to IEnumerable

            return deneme;
        }

        /*
                public override async Task AddAsync(Category item)
                {


                    if (item.CategoryName != null)
                    {
                        _iRep.AddAsync(item);
                        //return "Kategori eklendi";
                    }
                    //return "Kategori ismi girilmemiş";
                }*/
    }
}
