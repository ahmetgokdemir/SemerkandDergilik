using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Abstracts
{

    public interface IRepository<T> where T : EntityBase, IEntity
    {
        // Task AddAsync(T item) gibi yorum satırına alınan crud işlemler Token dersinden

        // List Commands
        // List<T> GetAll(); // IQueryable<T> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync();

        //List<T> GetActives();
        IQueryable<T> GetActivesAsync();
        // List<T> GetPassives();
        Task<IQueryable<T>> GetPassivesAsync();

        // List<T> GetModifieds();
        Task<IQueryable<T>> GetModifiedsAsync();


        //Modify Commands
        //void Add(T item);
        Task AddAsync(T entity);
        // void AddRange(List<T> list);
        Task AddRangeAsync(List<T> list);

        // void Delete(T item);
        void Delete(T entity);
        void DeleteRange(List<T> list);

        void Destroy(T entity);
        void DestroyRange(List<T> list);

        void Update(T entity); // T Update(T entity);
        void UpdateRange(List<T> list);

        //Linq
        // List<T> Where(Expression<Func<T, bool>> exp);  
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        bool Any(Expression<Func<T, bool>> exp);
        T FirstOrDefault(Expression<Func<T, bool>> exp);
        object Select(Expression<Func<T, object>> exp);
        object SelectViaClass<X>(Expression<Func<T, X>> exp);

        //Find Command
        // T Find(int id); // Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id);

        //Last Data
        // T GetLastData();
        Task<T> GetLastDataAsync();

        //First Data
        // T GetFirstData();
        Task<T> GetFirstDataAsync();

    }

}
