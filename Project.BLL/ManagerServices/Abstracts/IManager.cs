using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.ManagerServices.Abstracts
{
    public interface IManager<TEntity> where TEntity : class, IEntity 
    {        // Task AddAsync(T item) gibi yorum satırına alınan crud işlemler Token dersinden

        // List Commands
        // List<T> GetAll(); // IQueryable<T> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync();

        //List<T> GetActives();
        Task<IEnumerable<TEntity>> GetActivesAsync();
        // List<T> GetPassives();
        Task<IEnumerable<TEntity>> GetPassivesAsync();

        // List<T> GetModifieds();
        Task<IEnumerable<TEntity>> GetModifiedsAsync();


        //Modify Commands
        //void Add(T item);
        Task AddAsync(TEntity entity);
        // void AddRange(List<T> list);
        Task AddRangeAsync(List<TEntity> list);

        // void Delete(T item);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> list);

        void Destroy(TEntity entity);
        void DestroyRange(List<TEntity> list);

        void Update(TEntity entity); // T Update(T entity);
        void UpdateRange(List<TEntity> list);

        //Linq
        // List<T> Where(Expression<Func<T, bool>> exp);  
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> exp);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> exp);
        object Select(Expression<Func<TEntity, object>> exp);
        object SelectViaClass<X>(Expression<Func<TEntity, X>> exp);

        //Find Command
        // T Find(int id); // Task<T> GetByIdAsync(int id);
        Task<TEntity> GetByIdAsync(int id);

        //Last Data
        // T GetLastData();
        Task<TEntity> GetLastDataAsync();

        //First Data
        // T GetFirstData();
        Task<TEntity> GetFirstDataAsync();
    }
}
