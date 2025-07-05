using Microsoft.EntityFrameworkCore;
using SimpleAgenda.Context;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Entities;
using SimpleAgenda.Enums;
using SimpleAgenda.Repositories;


namespace SimpleAgendaTests.UnitTests.Repositories
{
    public class SqLiteRepositoryTest 
    {
    
        private const string DbPath = @"only-create.db";
           
        public static void Arrange()
        {
            if (File.Exists(DbPath))
                File.Delete(DbPath);
        }

        [Fact]
        public async Task SaveNewAppointment()
        {
            // Arrange
            Arrange();

            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var newAppointment = new AppointmentDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };

            // Act
            await repo.Create(newAppointment);

            // Assert
            var lista = await repo.GetList();
            Assert.Single(lista);
            Assert.NotNull(lista[0].Event);
        }

        [Fact]
        public async Task SaveNewEvent()
        {
            Arrange();

            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<EventDto>(context);
            var newEvent = new EventDto
            {
                Title = "Evento Público 3",
                Description = "Evento criado para teste"
            };
            // Act
            await repo.Create(newEvent);
            // Assert
            var lista = await repo.GetList();
            Assert.Single(lista);
        }

        [Fact]
        public async Task SaveNewLocation()
        {
            // Arrange
            if (File.Exists(DbPath))
                File.Delete(DbPath);
            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<LocationDto>(context);
            var newLocation = new LocationDto
            {
                Street = "Rua Teste",
                Number = "123",
                City = "Cidade Teste",
                PostalCode = "12345-678",
                Country = "País Teste",
                State = BrazilStatesEnum.MG,
                Complement = "Complemento Teste"
            };
            // Act
            await repo.Create(newLocation);
            // Assert
            var lista = await repo.GetList();
            Assert.Single(lista);
        }

        [Fact]
        public async Task GetAppointment()
        {
            Arrange();

            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var newAppointment = new AppointmentDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };

            await repo.Create(newAppointment);

            var appointment = await repo.Get(newAppointment.Id);

            Assert.NotNull(appointment);

        }

        [Fact]
        public async Task GetAllAppointments()
        {
            Arrange();

            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var newAppointment0 = new AppointmentDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };

            var newAppointment1 = new AppointmentDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };

            await repo.Create(newAppointment1);
            await repo.Create(newAppointment0);

            var appointments = await repo.GetList();

            Assert.NotNull(appointments);
            Assert.Equal(2,appointments.Count);

        }

        [Fact]
        public async Task DeleteAppointment()
        {
            Arrange();

            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var newAppointment0 = new AppointmentDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };

            var newAppointment1 = new AppointmentDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };

            await repo.Create(newAppointment1);
            await repo.Create(newAppointment0);

            await repo.Delete(newAppointment0.Id);

            var appointments = await repo.GetList();

            Assert.NotNull(appointments);
            Assert.Equal(1, appointments.Count);
            Assert.Equal(newAppointment1, appointments[0]);

        }

        [Fact]
        public async Task UpdateAppointment()
        {
            Arrange();

            var context = new SqliteContext($"Data Source={DbPath}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            DateTime updatedDate = DateTime.Today.AddDays(2);

            var newAppointment = new AppointmentDto
            {
                Date = DateTime.Today,
                Event = new EventDto
                {
                    Title = "Evento Público 3",
                    Description = "Evento criado para teste"
                }
            };
            await repo.Create(newAppointment);

            // Internal lógic to updating appointment
            Appointment appointment = new(newAppointment);

            var updatedAppointment = new AppointmentOutDto
            {
                Date = updatedDate
            };
            appointment.Update(updatedAppointment);

            // Persist the updated appointment back to the repository
            await repo.Update(appointment.Id, appointment.ConvertToInternalDto());

            // Retrieve the updated appointment from the repository to validate
            var toValidateAppointment = await repo.Get(appointment.Id);

            // Perform assertions to confirm the update
            Assert.NotNull(toValidateAppointment);
            Assert.Equal(appointment.Id, toValidateAppointment.Id);
            Assert.Equal(updatedDate, toValidateAppointment.Date);
        }

    }
}
