using Esti_bus_project.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public async Task<IEnumerable<T>> GetQuerableAsync(int items)
        {
            var query = _dbSet.Take(items);
            return await query.ToListAsync();
        }
    }
}
