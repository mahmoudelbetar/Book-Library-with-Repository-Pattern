using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace B_Gallery.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // T = Category
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includePropertis = null);
        IEnumerable<T> GetAll(string? includePropertis = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includePropertis = null);
        void Add(T entity);
        T GetById(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
