using Esti_bus_project.Models;

namespace Esti_bus_project.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetQuerableAsync(int items);
    }
}