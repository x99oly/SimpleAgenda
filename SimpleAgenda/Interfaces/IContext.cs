using Microsoft.EntityFrameworkCore;
using SimpleAgenda.DTOS.Internals;

namespace SimpleAgenda.Interfaces
{
    internal interface IContext
    {
        DbSet<EventDto> Events { get; }
        DbSet<AppointmentDto> Appointments { get; }
        DbSet<LocationDto> Locations { get; }
        void EnsureMigrated();
        void EnsureCreated();
        void EnsureDeleted();
    }
}
