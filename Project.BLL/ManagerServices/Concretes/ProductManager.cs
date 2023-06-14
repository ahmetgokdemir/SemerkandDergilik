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

        // IRepository (_iRep) ve IProductRepository (_prep) aynı constructor içeriside depency injection'a tabii tutuldular..
        public ProductManager(IRepository<Product> irep, IProductRepository prep) : base(irep)
        {
            _prep = prep;
        }

        // _iRep ve _prep ayrı constructor içerisinde dependency injection'a tabii tutulamadı..
        //public ProductManager(IProductRepository prep):base(prep)
        //{
        //    _prep = prep;
        //} 

        public async Task<IEnumerable<Product>> GetActivesProductsByCategory_of_FoodIDAsync(int Category_of_Food_id)
        {
            var products = await _prep.GetActivesProductsByCategory_of_FoodIDAsync(Category_of_Food_id).ToListAsync(); // convert ıqueryable to IEnumerable

            return products;
        }

        public async Task<Product> GetProductByIdwithCategory_of_FoodValueAsync(int product_id)
        {
            var product = await _prep.GetProductByIdwithCategory_of_FoodValueAsync(product_id).ToListAsync(); // convert ıqueryable to IEnumerable

            return product[0];
        }
    }
}
