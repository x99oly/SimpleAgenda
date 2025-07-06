using SimpleAgenda.Entities;
using SimpleAgenda.Enums;

namespace SimpleAgendaTest.UnitTests.Entities.Schedule
{
    public class ScheduleTests
    {
        [Fact]
        public void Should_ThrowException_When_StartDateIsInThePast()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddMinutes(-10);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new SimpleAgenda.Entities.Schedule(pastDate,10,0)
            );
        }

        [Fact]
        public void Should_UseDefaultEndDate_When_EndDateIsNotProvided()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var schedule = new SimpleAgenda.Entities.Schedule(now.AddMinutes(1), 8, 0);

            // Assert
            var expectedEnd = now.AddYears(100);
            Assert.True(schedule.StartAndEndRangeDates.EndDate > now.AddYears(99));
        }

        [Fact]
        public void Should_ThrowException_When_EndDateIsBeforeStartDate()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddHours(1);
            var endDate = startDate.AddMinutes(-1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new SimpleAgenda.Entities.Schedule(startDate, 9, 0, endDate)
            );
        }

        // Repete o teste com valores difetentes => InlineData é interval
        // Evita redundância de testes só com valores diferentes
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void Should_Accept_ValidRecurrenceInterval(int interval)
        {
            // Arrange
            var start = DateTime.UtcNow.AddMinutes(1);
            var schedule = new SimpleAgenda.Entities.Schedule(start, 7, 30, null, RecurrenceTypeEnum.DAILY, interval);

            // Assert
            Assert.Equal(interval, schedule.RecurrenceInterval);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_ThrowException_When_RecurrenceIntervalIsInvalid(int invalidInterval)
        {
            // Arrange
            var start = DateTime.UtcNow.AddMinutes(1);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                new SimpleAgenda.Entities.Schedule(start, 7, 30, null, RecurrenceTypeEnum.DAILY, invalidInterval)
            );
        }
    }
}
