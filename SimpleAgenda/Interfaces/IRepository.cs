
namespace SimpleAgenda.Interfaces
{
    public interface IRepository<T> where T : class
    {
         Task<T?> Get(int id);
         Task<List<T>> GetList();
         Task Create(T entity);
         Task Update(T entity);
         Task Delete(int id);
    }
}
