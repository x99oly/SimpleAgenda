using SimpleAgenda.Context;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Repositories;


namespace SimpleAgendaTests.UnitTests.Repositories
{
    public class SqLiteRepositoryTest
    {
        private const string DbPath = @"D:\LocalRepository\projetos\AppAgenda\SimpleAgendaTests\Public\Data\only-create.db";

        [Fact]
        public async Task SaveNewAppointment()
        {
            // Arrange
            if (File.Exists(DbPath))
                File.Delete(DbPath);

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
    }
}
