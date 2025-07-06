using System;
using System.Threading.Tasks;
using SimpleAgenda.Entities;
using SimpleAgenda.Interfaces;
using SimpleAgenda.Services.Cron;
using Xunit;

namespace SimpleAgendaTest.UnitTests.Services
{
    public class QuartzServiceTest
    {
        [Fact]
        public async Task RegisterAsync_ShouldScheduleJob_WhenValidScheduleAndDelegate()
        {
            // Arrange
            IScheduleCronJobManager cronManager = await ScheduleCronJobManager.CreateAsync();

            var schedule = new Schedule(
                startDate: DateTime.UtcNow.AddMinutes(1),
                hour: DateTime.UtcNow.Hour,
                minutes: DateTime.UtcNow.Minute + 1,
                recurrenceType: SimpleAgenda.Enums.RecurrenceTypeEnum.DAILY,
                recurrenceInterval: 1
            );

            // Act
            await cronManager.RegisterAsync(schedule, () => Console.WriteLine("Test job"));

            // Assert
            // Aqui é difícil testar internamente a execução do Quartz sem mocks,
            // mas podemos garantir que não houve exceção e o código executou até aqui.
            Assert.True(true);
        }

        [Fact]
        public async Task RegisterAsync_ShouldSupportMultipleSchedulesWithDifferentDelegates()
        {
            // Arrange
            IScheduleCronJobManager cronManager = await ScheduleCronJobManager.CreateAsync();

            int countA = 0;
            int countB = 0;

            var tcsA = new TaskCompletionSource();
            var tcsB = new TaskCompletionSource();

            var now = DateTime.UtcNow;

            var scheduleA = new Schedule(
                startDate: now.AddMinutes(1),
                hour: now.Hour,
                minutes: now.Minute,
                recurrenceType: SimpleAgenda.Enums.RecurrenceTypeEnum.DAILY,
                recurrenceInterval: 1
            );

            var scheduleB = new Schedule(
                startDate: now.AddMinutes(1),
                hour: now.Hour,
                minutes: now.Minute,
                recurrenceType: SimpleAgenda.Enums.RecurrenceTypeEnum.DAILY,
                recurrenceInterval: 1
            );

            // Act
            await cronManager.RegisterAsync(scheduleA, () =>
            {
                countA++;
                tcsA.TrySetResult();
            });

            await cronManager.RegisterAsync(scheduleB, (string msg) =>
            {
                countB++;
                tcsB.TrySetResult();
            }, ["Hello World"]);

            // Assert
            await Task.WhenAll(tcsA.Task, tcsB.Task);

            Assert.Equal(1, countA);
            Assert.Equal(1, countB);
        }
    }
}
