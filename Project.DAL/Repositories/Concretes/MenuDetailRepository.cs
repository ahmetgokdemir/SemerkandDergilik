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
    public class MenuDetailRepository : BaseRepository<MenuDetail>, IMenuDetailRepository
    {
        public class Deneme
        {
            public string FoodName { get; set; }
            public string CategoryName_of_Food { get; set; }
            public decimal UnitPrice { get; set; }


        }
        public MenuDetailRepository(TechnosoftProjectContext context) : base(context)
        {
        }

        public IQueryable<object> Get_FoodsofMenu_Async(int Menu_ID)
        {
            object asd = new Deneme();

            return _context.Set<Food>().Join(_context.Set<MenuDetail>(),
                (dc => dc.ID),
                (mr => mr.FoodID),
                (dc, mr) => new Deneme()
                {
                    FoodName = dc.FoodName,
                    CategoryName_of_Food = mr.CategoryName_of_Food,
                    UnitPrice = dc.UnitPrice
                }
                ).AsQueryable();

          
        }
    }
}
