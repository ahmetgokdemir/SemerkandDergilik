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
    public class MenuManager : BaseManager<Menu>, IMenuManager
    {
        //public MenuManager(IRepository<Menu> irep) : base(irep)
        //{
        //}

        IMenuRepository _mrep;

        public MenuManager(IRepository<Menu> irep, IMenuRepository mrep) : base(irep)
        {
            _mrep = mrep;
        }
    }
}
