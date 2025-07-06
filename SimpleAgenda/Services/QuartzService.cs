using Quartz;
using Quartz.Impl;
using SimpleAgenda.Entities;
using SimpleAgenda.Interfaces;
using System.Collections.Concurrent;

namespace SimpleAgenda.Services.Cron
{
    /// <summary>
    /// Gerenciador responsável por registrar e agendar schedules com execução automática.
    /// </summary>
    internal class ScheduleCronJobManager : IScheduleCronJobManager
    {
        private readonly IScheduler _scheduler;

        // Registra ações por ScheduleId
        internal static readonly ConcurrentDictionary<string, Func<Task>> JobHandlers = new();

        private ScheduleCronJobManager(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public static async Task<ScheduleCronJobManager> CreateAsync()
        {
            var factory = new StdSchedulerFactory();
            var scheduler = await factory.GetScheduler();
            await scheduler.Start();

            return new ScheduleCronJobManager(scheduler);
        }

        /// <summary>
        /// Registra e agenda um Schedule para execução única, usando sua StartDate + TimeOfDay.
        /// </summary>
        public async Task RegisterAsync(Schedule schedule, Delegate rawDelegate, object?[]? args = null)
        {
            if (rawDelegate is null)
                throw new ArgumentNullException(nameof(rawDelegate));

            var scheduleId = schedule.Id.ToString();

            // Wrap the delegate in a Func<Task>
            Func<Task> wrapped = () =>
            {
                object? result = rawDelegate.DynamicInvoke(args ?? Array.Empty<object>());
                return result is Task t ? t : Task.CompletedTask;
            };

            JobHandlers[scheduleId] = wrapped;

            var runDateTime = schedule.StartAndEndRangeDates.StartDate.Date
                + schedule.Recurrence.RecurrenceTime.AsTimeSpan();

            var job = JobBuilder.Create<ScheduleQuartzJob>()
                .WithIdentity(scheduleId)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{scheduleId}_trigger")
                .StartAt(runDateTime)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// Cancel the scheduled job and then remove it from handlers.
        /// </summary>
        public async Task CancelScheduleAsync(string scheduleId)
        {
            await _scheduler.DeleteJob(new JobKey(scheduleId));

            if (JobHandlers.TryRemove(scheduleId, out var _))
                await Task.CompletedTask;
                        
        }


    }

    /// <summary>
    /// Job executado pelo Quartz, que delega para a ação registrada via ScheduleId.
    /// </summary>
    internal class ScheduleQuartzJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var scheduleId = context.JobDetail.Key.Name;

            if (ScheduleCronJobManager.JobHandlers.TryGetValue(scheduleId, out var handler))
            {
                await handler.Invoke();
            }
        }
    }
}
