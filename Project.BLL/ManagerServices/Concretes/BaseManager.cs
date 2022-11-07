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
    public class BaseManager<TEntity> : IManager<TEntity> where TEntity : class, IEntity
    {
        protected readonly IRepository<TEntity> _iRep;

        public BaseManager(IRepository<TEntity> irep)
        {
            _iRep = irep;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _iRep.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> list)
        {
            await _iRep.AddRangeAsync(list);
        }

        public Task<bool> Any(Expression<Func<TEntity, bool>> exp)
        {
            return _iRep.Any(exp);
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
            _iRep.DeleteRange(list);
        }

        public void Destroy(TEntity entity)
        {
            _iRep.Destroy(entity);
        }

        public void DestroyRange(List<TEntity> list)
        {
            _iRep.DestroyRange(list);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> exp)
        {
            return await _iRep.FirstOrDefault(exp);
        }

        public async Task<IEnumerable<TEntity>> GetActivesAsync()
        {
            // IQueryable olması bize üzerinde linq kullanmamıza izin verir zira hala veri database üzerinde..
            // IEnumerable olunca veri db 'den çekilip memory'e set edilmiştir..
            return await _iRep.GetActivesAsync().ToListAsync(); // convert ıqueryable to IEnumerable
                         
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _iRep.GetAllAsync();

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

        public async Task<TEntity> GetFirstDataAsync()
        {
            return await _iRep.GetFirstDataAsync();
        }

        public async Task<TEntity> GetLastDataAsync()
        {
            return await _iRep.GetLastDataAsync();
        }

        public async Task<IEnumerable<TEntity>> GetModifiedsAsync()
        {
            return await _iRep.GetModifiedsAsync().ToListAsync(); // convert ıqueryable to IEnumerable

            // return products;
        }

        public async Task<IEnumerable<TEntity>> GetPassivesAsync()
        {
            return await _iRep.GetPassivesAsync().ToListAsync(); // convert ıqueryable to IEnumerable
        }

        public object Select(Expression<Func<TEntity, object>> exp)
        {
            return _iRep.Select(exp);
        }

        public object SelectViaClass<X>(Expression<Func<TEntity, X>> exp)
        {
            return _iRep.SelectViaClass<X>(exp);
        }

        public void Update(TEntity entity)
        {
            _iRep.Update(entity);
        }

        public void UpdateRange(List<TEntity> list)
        {
            _iRep.UpdateRange(list);
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _iRep.Where(predicate).ToListAsync();
        }
    }
}
