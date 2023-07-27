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
    public class CategoryofFoodManager : BaseManager<CategoryofFood>, ICategoryofFoodManager
    {
        ICategoryofFoodRepository _crep;
        // ICollection _icol;
        // Collection önce kave seç...

        // IRepository (_iRep) ve ICategoryofFoodRepository (_crep) aynı constructor içeriside dependency injection'a tabii tutuldular..
        // IRepository irep'nin metodlarını kullanabilmek için dependency injection'a tabii tutuldu (Inheritance/ : base(irep))

        /*         
        protected readonly IRepository<TEntity> _iRep;

        public BaseManager(IRepository<TEntity> irep) <--  base(irep)
        {
            _iRep = irep;
        }         
        */

        public CategoryofFoodManager(IRepository<CategoryofFood> irep, ICategoryofFoodRepository crep) : base(irep)
        {
            _crep = crep;
        }

        public async Task<IEnumerable<string>> GetActivesCategoryofFoodNamesAsync()
        {
            var CategoryofFoodNames = await _crep.GetActivesCategoryofFoodNamesAsync().ToListAsync(); // convert ıqueryable to IEnumerable

            return CategoryofFoodNames;
        }

        public async Task<string> GetCategoryofFoodNameAccordingToFoodAsync(short CategoryofFood_id)
        {
            var CategoryofFoodNameAccordingToFood = await _crep.GetCategoryofFoodNameAccordingToFoodAsync(CategoryofFood_id).ToListAsync(); // convert ıqueryable to IEnumerable
 
            return CategoryofFoodNameAccordingToFood[0];
         

        }

        /*
           public override async Task AddAsync(CategoryofFood item)
           {
              if (item.CategoryofFoodName != null)
              {
                  _iRep.AddAsync(item);
                  //return "Kategori eklendi";
              }

              //return "Kategori ismi girilmemiş";
           }*/
    }
}
