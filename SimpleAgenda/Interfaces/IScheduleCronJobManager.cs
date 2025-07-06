using SimpleAgenda.Entities;
using SimpleAgenda.Services.Cron;
// add changes
namespace SimpleAgenda.Interfaces
{
    /// <summary>
    /// Interface responsible for managing and registering schedules for automatic execution via cron.
    /// </summary>
    internal interface IScheduleCronJobManager
    {
        /// <summary>
        /// Initializes the manager and starts the internal scheduler.
        /// Should be called once before usage.
        /// </summary>
        /// <returns>An initialized instance of <see cref="ScheduleCronJobManager"/>.</returns>
        static Task<ScheduleCronJobManager> CreateAsync() => throw new NotImplementedException();
        
        /// <summary>
        /// Registers a <see cref="Schedule"/> to be executed at its start date and time.
        /// Accepts any delegate (synchronous or asynchronous), with or without parameters.
        /// </summary>
        /// <param name="schedule">The schedule to be registered.</param>
        /// <param name="rawDelegate">The delegate to execute when the schedule triggers.</param>
        /// <param name="args">Optional arguments to pass to the delegate, if any.</param>
        Task RegisterAsync(Schedule schedule, Delegate rawDelegate, object?[]? args = null);
    }
}
