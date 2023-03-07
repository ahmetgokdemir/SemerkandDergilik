using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Concretes
{
    public class CategoryManager : BaseManager<Category>, ICategoryManager
    {
        ICategoryRepository _crep;
        // ICollection _icol;
        // Collection önce kave seç...

        // IRepository (_iRep) ve ICategoryRepository (_crep) aynı constructor içeriside dependency injection'a tabii tutuldular..
        // IRepository irep'nin metodlarını kullanabilmek için dependency injection'a tabii tutuldu (Inheritance/ : base(irep))

        /*         
        protected readonly IRepository<TEntity> _iRep;

        public BaseManager(IRepository<TEntity> irep) <--  base(irep)
        {
            _iRep = irep;
        }         
        */

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
            var categoryNameAccordingToProduct = await _crep.GetCategoryNameAccordingToProductAsync(category_id).ToListAsync(); // convert ıqueryable to IEnumerable
 
            return categoryNameAccordingToProduct[0];
         

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
