using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{
    public interface IProductRepository : IRepository<Product>
    {
        IQueryable<Product> GetActivesProductsByCategoryIDAsync(int category_id);
    }

}
