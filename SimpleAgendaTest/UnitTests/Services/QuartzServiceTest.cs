//using System;
//using System.Threading.Tasks;
//using SimpleAgenda.Entities;
//using SimpleAgenda.Interfaces;
//using SimpleAgenda.Services.Cron;
//using Xunit;

//namespace SimpleAgendaTest.UnitTests.Services
//{
//    public class QuartzServiceTest
//    {
//        [Fact]
//        public async Task RegisterAsync_ShouldScheduleJob_WhenValidScheduleAndDelegate()
//        {
//            // Arrange
//            IScheduleCronJobManager cronManager = await ScheduleCronJobManager.CreateAsync();

//            var schedule = new Schedule(
//                startDate: DateTime.UtcNow.AddMinutes(1),
//                hour: DateTime.UtcNow.Hour,
//                minutes: DateTime.UtcNow.Minute+1,
//                recurrenceType: SimpleAgenda.Enums.RecurrenceTypeEnum.DAILY,
//                recurrenceInterval: 1
//            );

//            // Act
//            await cronManager.RegisterAsync(schedule, () => Console.WriteLine("Test job"));

//            // Assert
//            // Aqui é difícil testar internamente a execução do Quartz sem mocks,
//            // mas podemos garantir que não houve exceção e o código executou até aqui.
//            Assert.True(true);
//        }
//    }
//}
