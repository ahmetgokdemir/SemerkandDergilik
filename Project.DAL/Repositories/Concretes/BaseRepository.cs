using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Concretes
{
    public class BaseRepository<T> : IRepository<T> where T : EntityBase, IEntity
    {
        protected readonly DbContext _context;
        //private readonly DbSet<IEntity> _dbSet;

        public BaseRepository(SemerkandDergilikContext context)
        {
            _context = context;
            //_dbSet = context.Set<IEntity>();
        }

        void Save()
        {
            _context.SaveChanges();
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task AddAsync(T entity)
        {
            _context.Set<T>().AddAsync(entity);
            Save();
        }

        public Task AddRangeAsync(List<T> list)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public void Destroy(T entity)
        {
            _context.Set<T>().Remove(entity);
            Save();

        }

        public void DestroyRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetActivesAsync()
        {
            return _context.Set<T>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).AsQueryable(); ;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
            //return (IEnumerable<T>)await _dbSet.ToListAsync();
            //throw new NotImplementedException();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public Task<T> GetFirstDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetLastDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<T>> GetModifiedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<T>> GetPassivesAsync()
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            entity.DeletedDate = DateTime.Now;
            entity.DataStatus = ENTITIES.Enums.DataStatus.Deleted;

            var toBeUpdated = _context.Set<T>().Find(entity.ID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;
            _context.Entry(toBeUpdated).CurrentValues.SetValues(entity);
            Save();

        }

        public void DeleteRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public object Select(Expression<Func<T, object>> exp)
        {
            throw new NotImplementedException();
        }

        public object SelectViaClass<X>(Expression<Func<T, X>> exp)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            entity.DataStatus = ENTITIES.Enums.DataStatus.Updated;
            entity.ModifiedDate = DateTime.Now;
            // T toBeUpdated = Find(entity.ID);
            var toBeUpdated = _context.Set<T>().Find(entity.ID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;

            //if (toBeUpdated is Category /* || entity is Product*/ )
            //{
            //    Category c = toBeUpdated as Category;


            //}

            _context.Entry(toBeUpdated).CurrentValues.SetValues(entity);
            Save();
        }

        public void UpdateRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }
    }
}
