﻿using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Concretes
{
    public class MenuDetailRepository : BaseRepository<MenuDetail>, IMenuDetailRepository
    {
        public class MenuDetail_Repo
        {
            public string CategoryName_of_Food { get; set; }
            public string FoodName { get; set; }
            public decimal FoodPrice { get; set; }
            public string? FoodPicture { get; set; }
            public int Status { get; set; } // Aktif, Pasif

        }

        public class CategoriesOfMenu_Repo
        {
            public string Category_of_FoodName { get; set; }
             public int Status { get; set; } // Aktif, Pasif
        }

        public MenuDetailRepository(TechnosoftProjectContext context) : base(context)
        {
        }

        public IQueryable<object> Get_FoodsofMenu_Async(int Menu_ID)
        {
            //object asd = new Deneme();

            return _context.Set<MenuDetail>().Where(x => x.MenuID == Menu_ID).Join(_context.Set<Food>(),
                (md => md.FoodID),
                (fd => fd.ID),
                (md, fd) => new 
                {
                    CategoryName_of_Food = md.CategoryName_of_Food,
                    FoodName = fd.FoodName,
                    FoodPrice = fd.UnitPrice,
                    FoodPicture = fd.FoodPicture,
                    Status = fd.Status,
                }
                ).AsQueryable();
          
        }
 
        public IQueryable<CategoriesOfMenu_Repo> Get_CategoriesofMenu_Async(int Menu_ID)
        {
            IQueryable<CategoriesOfMenu_Repo> com_list;

            com_list = _context.Set<MenuDetail>().Where(x => x.MenuID == Menu_ID).Join(_context.Set<Category_of_Food>(),
                (md => md.CategoryName_of_Food),
                (cof => cof.Category_of_FoodName),
                (md, cof) => new CategoriesOfMenu_Repo()
                {
                    Category_of_FoodName = cof.Category_of_FoodName,
                    Status = cof.Status
                }
                ).Distinct().AsQueryable();
            
 
            return com_list;
        }

        public async Task<bool> IsExist_FoodinMenu_Repo_Async(int selected_foodID, int menu_ID)
        {
            bool food_exists = _context.Set<MenuDetail>().Any(x => x.MenuID == menu_ID && x.FoodID == selected_foodID);//.ToListAsync();

            return food_exists;
        }

        public void Insert_FoodonMenu_Repo_Async(int selected_foodID, string category_Name, int menu_ID)
        {

            MenuDetail menuDetail = new MenuDetail();
            menuDetail.MenuID = menu_ID;
            menuDetail.FoodID = selected_foodID;
            menuDetail.CategoryName_of_Food = category_Name;

            _context.Set<MenuDetail>().AddAsync(menuDetail);

        }
    }
}
