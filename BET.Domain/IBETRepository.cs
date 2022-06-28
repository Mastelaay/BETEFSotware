using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BET.Domain
{
    public interface IBETRepository<T>where T:class
    {
        DbContext DbContext { get; set; }
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
        IQueryable<T> GetByIQuerable(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}

