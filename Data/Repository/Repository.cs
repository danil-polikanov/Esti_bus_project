﻿using Esti_bus_project.IRepository;
using Esti_bus_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Esti_bus_project.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public DbSet<T> GetDbSet()
        {
            return _dbSet;
        }
        public async Task<IEnumerable<T>> GetQuerableAsync(int items)
        {
            if (items == 0)
            {
                var query = _context.Stops;
                return (IEnumerable<T>)await query.ToListAsync();
            }
            else
            {
                var query = _dbSet.Take(items);
                return await query.ToListAsync();
            }
       
        }
        public async Task<IEnumerable<TResult>> GetFilteredAsync<TResult>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> selector)
        {
            var query = _dbSet.Where(filter).Select(selector);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<TResult>> GetJoinedFilteredAsync<TJoin, TKey, TResult>(
            Expression<Func<T, bool>> filter,                   // Условие фильтрации для первой таблицы
            Expression<Func<T, TKey>> outerKeySelector,         // Поле для соединения в первой таблице
            Expression<Func<TJoin, TKey>> innerKeySelector,     // Поле для соединения во второй таблице
            Expression<Func<T, TJoin, TResult>> resultSelector, // Выражение для выборки результата
            DbSet<TJoin> joinDbSet                              // Вторая таблица для соединения
        ) where TJoin : class
        {
            try
            {
                var query = _dbSet
                    .Where(filter)
                    .Join(joinDbSet, outerKeySelector, innerKeySelector, resultSelector);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); throw;
            };
        }
    }
}
