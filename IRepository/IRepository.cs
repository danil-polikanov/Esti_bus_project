using Esti_bus_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Esti_bus_project.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetQuerableAsync(int items);
        Task<IEnumerable<TResult>> GetFilteredAsync<TResult>(
              Expression<Func<T, bool>> filter,
              Expression<Func<T, TResult>> selector);
        Task<IEnumerable<TResult>> GetJoinedFilteredAsync<TJoin, TKey, TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TJoin, TKey>> innerKeySelector, Expression<Func<T, TJoin, TResult>> resultSelector, DbSet<TJoin> joinDbSet) where TJoin : class;
        DbSet<T> GetDbSet();
      
    }
}