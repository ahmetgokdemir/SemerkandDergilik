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
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _context;
        //private readonly DbSet<IEntity> _dbSet;

        public BaseRepository(TechnosoftProjectContext context)
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
            await _context.Set<T>().AddAsync(entity);
            Save();
        }

        public async Task AddRangeAsync(List<T> list)
        {
            await _context.Set<T>().AddRangeAsync(list);
            Save();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Any(exp);
        }

        public void Destroy(T entity)
        {
            _context.Set<T>().Remove(entity);
            Save();

        }

        public void DestroyRange(List<T> list)
        {
            _context.Set<T>().RemoveRange(list);
            Save();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> exp)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(exp);
        }

        public IQueryable<T> GetActivesAsync()
        {
            // will call Where(Expression<Func<T, bool>> predicate) function
            return Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).AsQueryable();

            // 2.yol
            // return _context.Set<T>().Where(x => x.DataStatus != ENTITIES.Enums.DataStatus.Deleted).AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
            //return (IEnumerable<T>)await _dbSet.ToListAsync();
            //throw new NotImplementedException();

            //  // mongoDB: var categories = await _categoryCollection.Find(category => true).ToListAsync();  

        }

        public async Task<T> GetByIdAsync(short id)
        {
            // _context.Set<T>().Find(id);
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public async Task<T> GetFirstDataAsync()
        {
            return await _context.Set<T>().OrderBy(x => x.CreatedDate).FirstOrDefaultAsync();
        }

        public async Task<T> GetLastDataAsync()
        {
            return await _context.Set<T>().OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetModifiedsAsync()
        {
            // will call Where(Expression<Func<T, bool>> predicate) function
            return Where(x => x.DataStatus == ENTITIES.Enums.DataStatus.Updated).AsQueryable();
        }

        public IQueryable<T> GetPassivesAsync()
        {
            // will call Where(Expression<Func<T, bool>> predicate) function
            return Where(x => x.DataStatus == ENTITIES.Enums.DataStatus.Deleted).AsQueryable();
        }

        public void Delete(T entity)
        {
            entity.DeletedDate = DateTime.Now;
            entity.DataStatus = ENTITIES.Enums.DataStatus.Deleted;
            // entity.Status = Technosoft_Project.Enums.Status.Pasif;
            /*
                   
            entity.DeletedDate = DateTime.Now;
            entity.DataStatus = ENTITIES.Enums.DataStatus.Deleted;

            //T toBeUpdated = null;

            //if (entity is CategoryofFood || entity is Food || entity is Coupon)
            //{
            //    toBeUpdated = _context.Set<T>().Find(entity.Primary_ID);
            //}
            //else if (entity is AppRole || entity is AppRoleClaim)
            //{
            //    AppRole entity_2 = null;

            //    if (entity is AppRole)
            //    {
            //        entity_2 = entity as AppRole;
            //    }

            //    toBeUpdated = _context.Set<T>().Find(entity_2.Id);
            //}


            var toBeUpdated = _context.Set<T>().FindAsync(entity.Primary_ID) as T;
            _context.Entry(toBeUpdated).CurrentValues.SetValues(entity);
            Save();

       
             
             */

            // var toBeDeleted
            var toBeUpdated = _context.Set<T>().Find(entity.ID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;
            _context.Entry(toBeUpdated).CurrentValues.SetValues(entity);
            Save();

        }

        public void DeleteRange(List<T> list)
        {
            foreach (T item in list)
            {
                Delete(item);
            }
        }


        //public IQueryable<object> Select(Expression<Func<T, object>> exp)
        //{
        //    return _context.Set<T>().Select(exp).AsQueryable();
        //}

        //public async Task<object> Select(Expression<Func<T, object>> exp)
        //{
        //    return await _context.Set<T>().Select(exp).ToListAsync();
        //}

        public object Select(Expression<Func<T, object>> exp)
        {
            return _context.Set<T>().Select(exp).ToList();
        }

        //public async Task<object> SelectViaClass(Expression<Func<T, object>> exp)
        //{
        //    return await _context.Set<T>().Select(exp).FirstOrDefaultAsync();
        //}

        public object SelectViaClass<X>(Expression<Func<T, X>> exp)
        {
            return _context.Set<T>().Select(exp).FirstOrDefault();
        }


        public void Update(T entity)
        {
            entity.DataStatus = ENTITIES.Enums.DataStatus.Updated;
            entity.ModifiedDate = DateTime.Now;
            // T toBeUpdated = Find(entity.ID);
            var toBeUpdated = _context.Set<T>().Find(entity.ID);
            // var toBeUpdated = _context.Set<T>().FindAsync(entity.ID) as T;

            //if (toBeUpdated is CategoryofFood /* || entity is Food*/ )
            //{
            //    CategoryofFood c = toBeUpdated as CategoryofFood;


            //}

            _context.Entry(toBeUpdated).CurrentValues.SetValues(entity);
            Save();
        }

        public void UpdateRange(List<T> list)
        {
            foreach (T item in list)
            {
                Update(item);
            }
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }
    }
}
