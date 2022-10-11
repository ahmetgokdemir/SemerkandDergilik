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
        public CategoryManager(IRepository<Category> irep) : base(irep)
        {
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
