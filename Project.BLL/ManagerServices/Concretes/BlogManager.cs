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
    public class BlogManager : BaseManager<Blog>, IBlogManager
    {
        //IBlogRepository _brep;

        public BlogManager(IRepository<Blog> irep/*, IBlogRepository brep*/) : base(irep)
        {
            //_brep = brep;
        }
    }
}
