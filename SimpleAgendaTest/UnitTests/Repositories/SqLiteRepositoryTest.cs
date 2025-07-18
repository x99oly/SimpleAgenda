﻿/*
 The following code has an implementation problem. It is using various intances of SqlLiteRepositoryTest to test the repository functionality while the test functions are running in parallel. This may cause the tests to not run all at once. 
May the solution is to use a single instance of SqlLiteRepositoryTest and run the tests sequentially.
*/
using SimpleAgenda.Context;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Entities;
using SimpleAgenda.Enums;
using SimpleAgenda.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleAgendaTest.UnitTests.Repositories
{
    public class SqLiteRepositoryTest : IAsyncLifetime
    {
    
        private string DbPath = string.Empty;
        
        public Task InitializeAsync()
        {
            // Obtém o diretório onde o código está sendo executado
            string currentDirectory = Directory.GetCurrentDirectory();

            // Caminho para o diretório 'UnitTests' (um nível acima do diretório de execução)
            string unitTestsDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", "..", "UnitTests"));

            // Caminho completo para a pasta 'Public' dentro de 'UnitTests'
            string publicDirectory = Path.Combine(unitTestsDirectory, "Public");

            // Verifica se a pasta "public" já existe. Se não, cria.
           
            var files = Directory.GetFiles(publicDirectory);
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file); // Apaga o arquivo
                    Console.WriteLine($"Arquivo {file} deletado.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao deletar o arquivo {file}: {ex.Message}");
                }
            }
            

            DbPath = publicDirectory;

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            //Directory.Delete(DbPath, true); // Remove a pasta se já existir
            return Task.CompletedTask;
        }

        [Fact]
        public async Task SaveNewAppointment()
        {
            string dbfile = "SaveNewAppointmentFuncion.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
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
            string dbfile = "SaveNewEvent.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
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
            string dbfile = "SaveNewLocation.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<LocationDto>(context);
            var newLocation = new LocationDto
            {
                Street = "Rua Teste",
                Number = "123",
                City = "Cidade Teste",
                PostalCode = "12345-678",
                Country = "País Teste",
                State = ['M','G'],
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
            string dbfile = "GetAppointment.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
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
            string dbfile = "GetAllAppointments.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
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
            Assert.Equal(2, appointments.Count);

        }

        [Fact]
        public async Task DeleteAppointment()
        {
            string dbfile = "SaveNewAppointment.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
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
            Assert.Single(appointments);
            Assert.Equal(newAppointment1, appointments[0]);

        }

        [Fact]
        public async Task UpdateAppointment()
        {
            string dbfile = "UpdateAppointment.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
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

        [Fact]
        public async Task GetListWithFilteringByAppointmentId()
        {
            string dbfile = "GetListWithFilteringByAppointmentId.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 10,
                Date = DateTime.Today.AddDays(1),
                Event = new EventDto
                {
                    Id = 100,
                    Title = "Evento A",
                    Description = "Teste 1"
                }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 20,
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Id = 200,
                    Title = "Evento B",
                    Description = "Teste 2"
                }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);

            // Act - filtro por Id que existe
            var filterExists = new QueryDto { AppointmentId = 10 };
            var resultsExists = await repo.GetList<Appointment>(filterExists);

            // Assert - deve retornar só o que tem o Id solicitado
            Assert.NotNull(resultsExists);
            Assert.Single(resultsExists);
            Assert.Equal("Evento A", resultsExists[0].Event.Title);

            // Act - filtro por Id que não existe
            var filterNotExists = new QueryDto { AppointmentId = 999999 };
            var resultsNotExists = await repo.GetList<Appointment>(filterNotExists);

            // Assert - não deve retornar nenhum registro
            Assert.NotNull(resultsNotExists);
            Assert.Empty(resultsNotExists);
        }
               
        [Fact]
        public async Task GetListWithFilteringByStatus()
        {
            string dbfile = "GetListWithFilteringByStatus.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Status = StatusEnum.ACTIVE,
                Event = new EventDto { Id = 10, Title = "Evento A", Description = "Desc A" }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 2,
                Date = DateTime.Today.AddDays(2),
                Status = StatusEnum.RESCHEDULED,
                Event = new EventDto { Id = 20, Title = "Evento B", Description = "Desc B" }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);

            var filterPending = new QueryDto { StatusIn = [StatusEnum.ACTIVE] };
            var resultsPending = await repo.GetList<Appointment>(filterPending);

            Assert.NotNull(resultsPending);
            Assert.Single(resultsPending);
            Assert.Equal(StatusEnum.ACTIVE, resultsPending[0].Status);

            var filterCancelled = new QueryDto { StatusIn = [StatusEnum.CANCELED] };
            var resultsCancelled = await repo.GetList<Appointment>(filterCancelled);

            Assert.NotNull(resultsCancelled);
            Assert.Empty(resultsCancelled);
        }

        [Fact]
        public async Task GetListWithFilteringByStatusIn()
        {
            string dbfile = "GetListWithFilteringByStatusIn.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Status = StatusEnum.ACTIVE,
                Event = new EventDto { Id = 10, Title = "Evento A", Description = "Desc A" }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 2,
                Date = DateTime.Today.AddDays(2),
                Status = StatusEnum.ACTIVE,
                Event = new EventDto { Id = 20, Title = "Evento B", Description = "Desc B" }
            };

            var appointment2 = new AppointmentDto
            {
                Id = 3,
                Date = DateTime.Today.AddDays(3),
                Status = StatusEnum.CANCELED,
                Event = new EventDto { Id = 30, Title = "Evento C", Description = "Desc C" }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);
            await repo.Create(appointment2);

            var filter = new QueryDto
            {
                StatusIn = [StatusEnum.ACTIVE, StatusEnum.ACTIVE]
            };

            var results = await repo.GetList<Appointment>(filter);

            Assert.NotNull(results);
            Assert.Equal(2, results.Count);
            Assert.DoesNotContain(results, a => a.Status == StatusEnum.CANCELED);
        }
                
        [Fact]
        public async Task GetListWithFilteringByEventTitle()
        {
            string dbfile = "GetListWithFilteringByEventTitle.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Event = new EventDto { Id = 10, Title = "Meeting with Team", Description = "Desc A" }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 2,
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto { Id = 20, Title = "Project Kickoff", Description = "Desc B" }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);

            var filter = new QueryDto
            {
                EventTitle = "Meeting"
            };

            var results = await repo.GetList<Appointment>(filter);

            Assert.NotNull(results);
            Assert.Single(results);
            Assert.Contains("Meeting", results[0].Event.Title);
        }

        [Fact]
        public async Task GetListWithFilteringByEventDescription()
        {
            string dbfile = "GetListWithFilteringByEventDescription.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Event = new EventDto { Id = 10, Title = "Event A", Description = "Annual conference" }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 2,
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto { Id = 20, Title = "Event B", Description = "Monthly meeting" }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);

            var filter = new QueryDto
            {
                EventDescription = "conference"
            };

            var results = await repo.GetList<Appointment>(filter);

            Assert.NotNull(results);
            Assert.Single(results);
            Assert.Contains("conference", results[0].Event.Description);
        }

        [Fact]
        public async Task GetListWithFilteringByLocationCity()
        {
            string dbfile = "GetListWithFilteringByLocationCity.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Event = new EventDto
                {
                    Id = 10,
                    Title = "Event A",
                    Description = "Desc A",
                    Location = new LocationDto
                    {
                        Street = "Street A",
                        Number = "1",
                        City = "CityX",
                        PostalCode = "00001",
                        Country = "CountryA",
                        State = ['S','P']
                    }
                }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 2,
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Id = 20,
                    Title = "Event B",
                    Description = "Desc B",
                    Location = new LocationDto
                    {
                        Street = "Street B",
                        Number = "2",
                        City = "CityY",
                        PostalCode = "00002",
                        Country = "CountryB",
                        State = ['R','J']
                    }
                }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);

            var filter = new QueryDto
            {
                City = "CityX"
            };

            var results = await repo.GetList<Appointment>(filter);

            Assert.NotNull(results);
            Assert.NotNull(results[0]);
            Assert.NotNull(results[0].Event);
            Assert.NotNull(results[0].Event.Location);
            Assert.Equal("CityX", results[0].Event.Location!.City);
            Assert.Single(results);
        }

        [Fact]
        public async Task GetListWithFilteringBySearchTerm()
        {
            string dbfile = "GetListWithFilteringBySearchTerm.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            var appointment0 = new AppointmentDto
            {
                Id = 1,
                Date = DateTime.Today.AddDays(1),
                Event = new EventDto
                {
                    Id = 10,
                    Title = "Team Meeting",
                    Description = "Discuss project goals"
                }
            };

            var appointment1 = new AppointmentDto
            {
                Id = 2,
                Date = DateTime.Today.AddDays(2),
                Event = new EventDto
                {
                    Id = 20,
                    Title = "Client Call",
                    Description = "Monthly update"
                }
            };

            await repo.Create(appointment0);
            await repo.Create(appointment1);

            var filter = new QueryDto
            {
                SearchTerm = "project"
            };

            var results = await repo.GetList<Appointment>(filter);

            Assert.NotNull(results);
            Assert.Single(results);
            Assert.Contains("project", results[0].Event.Description);
        }

        [Fact]
        public async Task GetListWithPagination()
        {
            string dbfile = "GetListWithPagination.db";
            var context = new SqliteContext($"Data Source={DbPath}/{dbfile}");
            var repo = new AgendaRepository<AppointmentDto>(context);

            for (int i = 1; i <= 5; i++)
            {
                var appointment = new AppointmentDto
                {
                    Id = i,
                    Date = DateTime.Today.AddDays(i),
                    Event = new EventDto
                    {
                        Id = i * 10,
                        Title = $"Event {i}",
                        Description = $"Description {i}"
                    }
                };
                await repo.Create(appointment);
            }

            var filter = new QueryDto
            {
                Skip = 1,
                Take = 2,
                OrderBy = "Date"
            };

            var results = await repo.GetList<Appointment>(filter);

            Assert.NotNull(results);
            Assert.Equal(2, results.Count);
            Assert.Equal(2, results[0].Id);  // Skip 1 (Id=1), next is Id=2
            Assert.Equal(3, results[1].Id);
        }

    }
}
