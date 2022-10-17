﻿using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface ICategoryManager : IManager<Category>
    {

        Task<IEnumerable<string>> GetActivesCategoryNamesAsync();
        Task<string> GetCategoryNameAccordingToProductAsync(int category_id);


        /*
         
                 public IQueryable<T> GetActivesAsync()
        {
            return _context.Set<T>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).AsQueryable(); ;
        }
         
         */

    }
}
