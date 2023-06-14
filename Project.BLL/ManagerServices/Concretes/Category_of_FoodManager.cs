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
    public class Category_of_FoodManager : BaseManager<Category_of_Food>, ICategory_of_FoodManager
    {
        ICategory_of_FoodRepository _crep;
        // ICollection _icol;
        // Collection önce kave seç...

        // IRepository (_iRep) ve ICategory_of_FoodRepository (_crep) aynı constructor içeriside dependency injection'a tabii tutuldular..
        // IRepository irep'nin metodlarını kullanabilmek için dependency injection'a tabii tutuldu (Inheritance/ : base(irep))

        /*         
        protected readonly IRepository<TEntity> _iRep;

        public BaseManager(IRepository<TEntity> irep) <--  base(irep)
        {
            _iRep = irep;
        }         
        */

        public Category_of_FoodManager(IRepository<Category_of_Food> irep, ICategory_of_FoodRepository crep) : base(irep)
        {
            _crep = crep;
        }

        public async Task<IEnumerable<string>> GetActivesCategory_of_FoodNamesAsync()
        {
            var Category_of_FoodNames = await _crep.GetActivesCategory_of_FoodNamesAsync().ToListAsync(); // convert ıqueryable to IEnumerable

            return Category_of_FoodNames;
        }

        public async Task<string> GetCategory_of_FoodNameAccordingToProductAsync(int Category_of_Food_id)
        {
            var Category_of_FoodNameAccordingToProduct = await _crep.GetCategory_of_FoodNameAccordingToProductAsync(Category_of_Food_id).ToListAsync(); // convert ıqueryable to IEnumerable
 
            return Category_of_FoodNameAccordingToProduct[0];
         

        }

        /*
           public override async Task AddAsync(Category_of_Food item)
           {
              if (item.Category_of_FoodName != null)
              {
                  _iRep.AddAsync(item);
                  //return "Kategori eklendi";
              }

              //return "Kategori ismi girilmemiş";
           }*/
    }
}
