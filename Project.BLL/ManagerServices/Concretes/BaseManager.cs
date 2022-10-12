using Microsoft.EntityFrameworkCore;
using Project.BLL.ManagerServices.Abstracts;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Concretes
{
    public class BaseManager<TEntity> : IManager<TEntity> where TEntity : EntityBase, IEntity
    {
        protected readonly IRepository<TEntity> _iRep;

        public BaseManager(IRepository<TEntity> irep)
        {
            _iRep = irep;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _iRep.AddAsync(entity);
        }

        public Task AddRangeAsync(List<TEntity> list)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<TEntity, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            //var isExistEntity = await _iRep.GetByIdAsync(id);


            //if (isExistEntity == null)
            //{
            //    //return "";
            //}

            _iRep.Delete(entity);
        }

        public void DeleteRange(List<TEntity> list)
        {
            throw new NotImplementedException();
        }

        public void Destroy(TEntity entity)
        {
            _iRep.Destroy(entity);
        }

        public void DestroyRange(List<TEntity> list)
        {
            throw new NotImplementedException();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetActivesAsync()
        {
            var products = await _iRep.GetActivesAsync().ToListAsync(); // convert ıqueryable to IEnumerable

            return products;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var products = await _iRep.GetAllAsync();

            return products;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var product = await _iRep.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            return product;
        }

        public Task<TEntity> GetFirstDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetLastDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetModifiedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetPassivesAsync()
        {
            throw new NotImplementedException();
        }

        public object Select(Expression<Func<TEntity, object>> exp)
        {
            throw new NotImplementedException();
        }

        public object SelectViaClass<X>(Expression<Func<TEntity, X>> exp)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            _iRep.Update(entity);
        }

        public void UpdateRange(List<TEntity> list)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
