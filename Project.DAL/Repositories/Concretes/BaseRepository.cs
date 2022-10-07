using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.DAL.Repositories.Abstracts;
using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Concretes
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<IEntity> _dbSet;

        public BaseRepository(SemerkandDergilikContext context)
        {
            _context = context;
            _dbSet = context.Set<IEntity>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task AddRangeAsync(List<T> list)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public void Destroy(T item)
        {
            throw new NotImplementedException();
        }

        public void DestroyRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<T>> GetActivesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            //return _context.Set<T>().ToListAsync();
            return (IEnumerable<T>)await _dbSet.ToListAsync();
            //throw new NotImplementedException();

        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(List<T> list)
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

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(List<T> list)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
