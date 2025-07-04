using SimpleAgenda.Enums;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Services;

namespace SimpleAgendaTests.UnitTests.Entities.Appointment
{
    public class AppointmentServicePublicDtoTests
    {
        private readonly AppointmentService<AppointmentOutDto> _service;

        public AppointmentServicePublicDtoTests()
        {            
            var repository = new InMemoryRepository<AppointmentOutDto>();
            _service = new AppointmentService<AppointmentOutDto>(repository);
        }

        [Fact]
        public async Task Create_ValidAppointment_ReturnsCreatedId()
        {
            var newAppointment = new AppointmentOutDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventOutDto { Title = "Evento Público 3" }
            };
            int createdId = await _service.Create(newAppointment);
            Assert.True(createdId > 0);
        }

        [Fact]
        public async Task Create_NullAppointment_ThrowsInvalidCastException()
        {
            await Assert.ThrowsAsync<InvalidCastException>(() => _service.Create(null!));
        }

        [Fact]
        public async Task Create_NullEvent_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.Create(
                new AppointmentOutDto
                {
                    Id = 1234,
                    Date = DateTime.Today.AddDays(2),
                    Event = null!
                }
                ));
        }

        [Fact]
        public async Task Create_NewAppointmentWhithInvalidPreviousDate_ThrowsArgumentException()
        {
            var newAppointment = new AppointmentOutDto
            {
                Date = DateTime.Today.Subtract(TimeSpan.FromDays(2)),
                Event = new EventOutDto { Title = "Evento Público 3" }
            };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.Create(newAppointment));
        }

        [Fact]
        public async Task Create_NullDate_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.Create(
                    new AppointmentOutDto
                    {
                        Id = 1234,
                        Date = null!,
                        Event = new EventOutDto
                        {
                            Title = "Evento público 4."
                        }
                    }
                ));
        }

        [Fact]
        public async Task Get_Returns_Correct_AppointmentOutDto()
        {
            string title = "Random Event 0";
            DateTime eventDate = DateTime.Today.AddDays(2);

            var newAppointment = new AppointmentOutDto
            {
                Date = eventDate,
                Event = new EventOutDto { Title = title }
            };
            int createdId = await _service.Create(newAppointment);

            var result = await _service.Get(createdId); // Ajuste se tiver Id, veja abaixo

            Assert.NotNull(result);
            Assert.Equal(title, result.Event.Title);
            Assert.Equal(eventDate, result.Date);
        }

        [Fact]
        public async Task GetList_Returns_All_AppointmentOutDtos()
        {
            string t0 = string.Empty, t1 = string.Empty;
            
            t0 = "List Event 0";
            DateTime eventDate = DateTime.Today.AddDays(2);

            var newAppointment = new AppointmentOutDto
            {
                Date = eventDate,
                Event = new EventOutDto { Title = t0 }
            };
            int createdId = await _service.Create(newAppointment);

            t1 = "List Event 1";
            eventDate = DateTime.Today.AddDays(2);

            newAppointment = new AppointmentOutDto
            {
                Date = eventDate,
                Event = new EventOutDto { Title = t1 }
            };
            createdId = await _service.Create(newAppointment);


            var list = await _service.GetList();

            Assert.True(list.Count >= 2);
            Assert.Contains(list, dto => dto.Event.Title == t0);
            Assert.Contains(list, dto => dto.Event.Title == t1);
        }

        [Fact]
        public async Task Update_Update_Appointment_Date()
        {
            DateTime updateDate = DateTime.Today.AddDays(5);

            var newAppointment = new AppointmentOutDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventOutDto { Title = "Evento Público 3" }
            };
            int createdId = await _service.Create(newAppointment);

            var updatedAppointment = new AppointmentOutDto
            {
                Date = updateDate            
            };

            await _service.Update(createdId, updatedAppointment);

            var appointment = await _service.Get(createdId);

            Assert.NotNull(appointment);
            Assert.Equal(updateDate, appointment.Date);

        }

        [Fact]
        public async Task Update_Update_Event_Title()
        {
            var newAppointment = new AppointmentOutDto
            {
                Date = DateTime.Now.AddHours(1),
                Event = new EventOutDto { Title = "Título Original" }
            };
            int createdId = await _service.Create(newAppointment);

            var updatedAppointment = new AppointmentOutDto
            {
                Event = new EventOutDto
                {
                    Title = "Título Atualizado"
                }
            };

            await _service.Update(createdId, updatedAppointment);

            var appointment = await _service.Get(createdId);

            Assert.NotNull(appointment);
            Assert.Equal("Título Atualizado", appointment.Event.Title);
        }

        [Fact]
        public async Task Update_Update_Location_Only()
        {
            var newAppointment = new AppointmentOutDto
            {
                Date = DateTime.Now.AddHours(1),
                Event = new EventOutDto
                {
                    Title = "Com Endereço Original",
                    Location = new LocationOutDto
                    {
                        Street = "Rua Atualizada",
                        Number = "200",
                        City = "Cidade Atualizada",
                        PostalCode = "11111-111",
                        Country = "Brasil",
                        State = BrazilStatesEnum.SP
                    }

                }
            };
            int createdId = await _service.Create(newAppointment);

            var updatedAppointment = new AppointmentOutDto
            {
                Event = new EventOutDto
                {
                    Location = new LocationOutDto
                    {
                        Street = "Rua Atualizada",
                        City = "Cidade Atualizada"
                    }
                }
            };

            await _service.Update(createdId, updatedAppointment);

            var appointment = await _service.Get(createdId);

            Assert.NotNull(appointment);
            Assert.Equal("Rua Atualizada", appointment.Event.Location.Street);
            Assert.Equal("Cidade Atualizada", appointment.Event.Location.City);
        }

        [Fact]
        public async Task Update_All_Fields()
        {
            var todayDate = DateTime.Now.AddHours(1);
            var updatedDate = DateTime.Today.AddDays(10);

            var newAppointment = new AppointmentOutDto
            {
                Date = todayDate,
                Event = new EventOutDto
                {
                    Title = "Original",
                    Description = "Descrição Original",
                    Location = new LocationOutDto
                    {
                        Street = "Rua A",
                        Number = "100",
                        City = "Cidade A",
                        PostalCode = "00000-000",
                        Country = "Brasil",
                        State = BrazilStatesEnum.SP
                    }

                }
            };
            int createdId = await _service.Create(newAppointment);

            var updatedAppointment = new AppointmentOutDto
            {
                Date = updatedDate,
                Event = new EventOutDto
                {
                    Title = "Novo Título",
                    Description = "Nova Descrição",
                    Location = new LocationOutDto
                    {
                        Street = "Rua Nova",
                        City = "Cidade Nova"
                    }
                }
            };

            await _service.Update(createdId, updatedAppointment);

            var appointment = await _service.Get(createdId);

            Assert.NotNull(appointment);
            Assert.Equal(updatedDate, appointment.Date);
            Assert.Equal("Novo Título", appointment.Event.Title);
            Assert.Equal("Nova Descrição", appointment.Event.Description);
            Assert.Equal("Rua Nova", appointment.Event.Location.Street);
            Assert.Equal("Cidade Nova", appointment.Event.Location.City);
        }

        [Fact]
        public async Task Delete_DeleteExistingAppointment()
        {
            var newAppointment = new AppointmentOutDto
            {
                Date = DateTime.Today.AddDays(2),
                Event = new EventOutDto { Title = "Evento Público 3" }
            };
            int createdId = await _service.Create(newAppointment);
            await _service.Delete(createdId);
            var result = await _service.Get(createdId);

            Assert.NotNull(result);
            Assert.Null(result.Date);
        }

        [Fact]
        public async Task Delete_DeleteNonExistingAppointment()
        {
            int nonExistingId = 9999; // Id que não existe
            await _service.Delete(nonExistingId);
            var result = await _service.Get(nonExistingId);

            Assert.NotNull(result);
            Assert.Null(result.Date); // Deve retornar um DTO vazio ou nulo
        }

    }
}
