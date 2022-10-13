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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {

        public ProductRepository(SemerkandDergilikContext context) : base(context)
        {

        }

        public IQueryable<Product> GetActivesProductsByCategoryIDAsync(int category_id)
        {
            return _context.Set<Product>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted && x.CategoryID == category_id).Include(x=> x.Category).AsQueryable(); ;
        }

    }
}
