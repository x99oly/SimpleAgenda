/* 
 * CTRL + M + O to collapse all regions *
 * 
The meaning of this classes is to encapsulate the logic of the management of appointments in the agenda.
Here we have a service that interacts with the repository to perform CRUD operations on appointments.
This class is designed to only expose the public DTOs, so it can be used in the API layer or any other layer that needs to interact with appointments.
 */

using SimpleAgenda.Entities;
using SimpleAgenda.Context;
using SimpleAgenda.Interfaces;
using SimpleAgenda.Repositories;
using SimpleAgenda.DTOS.Publics;

namespace SimpleAgenda.Services
{
    public class AppointmentService<T> where T : AppointmentOutDto
    {
        private readonly IRepository<T> _repository;

        /// <summary>
        /// This constructor initializes the service with a default repository.
        /// If you want to use a custom repository, you must grantee it implements IRepository<T> and the passign it as parameter.
        /// </summary>
        public AppointmentService()
        {
            _repository = new AgendaRepository<T>(new SqliteContext());
        }

        /// <summary>
        /// This constructor allows to inject a custom repository.
        /// Repository must implement IRepository<T>.
        /// Repositories are used to perform CRUD operations on the database.
        /// </summary>
        /// <param name="repository">The personal repository</param>
        public AppointmentService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<AppointmentOutDto?> Get(int id)
        {
            T? result = await PrivateGet(id);

            if (result == null)
                return new AppointmentOutDto();

            return result;
        }

        public async Task<List<AppointmentOutDto>> GetList()
        {
            List<T> results = await _repository.GetList();

            return results
                .Select(item =>
                {
                    return item as AppointmentOutDto;
                })
                .ToList();
        }

        public async Task<int> Create(T entity)
        {

            AppointmentOutDto apot = entity as AppointmentOutDto
            ?? throw new InvalidCastException($"The type passed does not match the signature of 'AppointmentOutDto'.");

            DateTime date = apot.Date ?? throw new ArgumentNullException(nameof(apot.Date), $"The parameter 'Date' cannot be null.");

            AppointmentOutDto ap = new Appointment(date, new Event(apot.Event)).ConvertToPublicDto();

            T? apt = ap as T;
            if (apt is null) return 0;

            await _repository.Create(apt);

            return ap.Id;
        }

        public async Task Update(int id, AppointmentOutDto entity)
        {
            T? recoverEntity = await PrivateGet(id);

            if (recoverEntity == null)
                throw new KeyNotFoundException($"Entity with ID {id} not found.");

            Appointment? ap = new Appointment(recoverEntity as AppointmentOutDto);

            if (ap == null)
                throw new InvalidCastException("The entity cannot be converted to an Appointment.");

            // Update the appointment with the new data
            ap.Update(entity);

            T? internalDto = ap.ConvertToPublicDto() as T 
                ?? throw new InvalidCastException("The converted AppointmentOutDto is null or not of the expected type.");
            
            await _repository.Update(ap.Id, internalDto as T);
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }


        private async Task<T?> PrivateGet(int id) => await _repository.Get(id) ?? null;

    }
}
