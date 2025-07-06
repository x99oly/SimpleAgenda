using SimpleAgenda.Entities;

namespace SimpleAgenda.Interfaces
{
    /// <summary>
    /// Interface para registrar ações a serem executadas por um Schedule.
    /// </summary>
    internal interface IScheduleAction
    {
        Task ExecuteAsync(Schedule schedule);
    }
}
