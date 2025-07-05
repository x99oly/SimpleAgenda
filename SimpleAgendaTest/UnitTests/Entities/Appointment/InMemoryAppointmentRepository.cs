using SimpleAgenda.Interfaces;

namespace SimpleAgendaTests.UnitTests.Entities.Appointment
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly List<T> _data;

        public InMemoryRepository(List<T>? initialData = null)
        {
            _data = initialData ?? new List<T>();
        }

        public Task<T?> Get(int id)
        {
            var entity = _data.FirstOrDefault(e => GetEntityId(e) == id);
            return Task.FromResult(entity);
        }

        public Task<List<T>> GetList()
        {
            return Task.FromResult(_data.ToList()); // Cópia defensiva
        }

        public Task Create(T entity)
        {
            _data.Add(entity);
            return Task.CompletedTask;
        }

        public Task Update(int id, T entity)
        {
            var index = _data.FindIndex(e => GetEntityId(e) == GetEntityId(entity));
            if (index >= 0)
                _data[index] = entity;
            return Task.CompletedTask;
        }

        public Task Delete(int id)
        {
            _data.RemoveAll(e => GetEntityId(e) == id);
            return Task.CompletedTask;
        }

        // Método para obter o Id da entidade, que deve ser implementado conforme o tipo T
        private int GetEntityId(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Id");
            if (propertyInfo == null)
                throw new InvalidOperationException("Tipo T deve conter a propriedade 'Id'.");

            return (int)propertyInfo.GetValue(entity)!;
        }
    }
}
