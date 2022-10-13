using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Concretes
{
    public class ProductManager : BaseManager<Product>, IProductManager
    {
        IProductRepository _prep;

        public ProductManager(IRepository<Product> irep, IProductRepository prep) : base(irep)
        {
            _prep = prep;
        }



        //public ProductManager(IProductRepository prep):base(prep)
        //{
        //    _prep = prep;
        //} 

        public async Task<IEnumerable<Product>> GetActivesProductsByCategoryIDAsync(int category_id)
        {
            var products = await _prep.GetActivesProductsByCategoryIDAsync(category_id).ToListAsync(); // convert ıqueryable to IEnumerable

            return products;
        }
    }
}
