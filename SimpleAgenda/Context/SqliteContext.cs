/*
This class implements a DbContext for a SQLite database using Entity Framework Core.
It defines DbSets for EventDto, AppointmentDto, and LocationDto,
It configures the database connection string,
It ensures the database is migrated, created, or deleted as needed,
It configures the model relationships and keys for the entities.
It also not build to be directly exposed to the user, but rather used internally by the application.
To configure will be set a configuration class to all it needs.
*/

using Microsoft.EntityFrameworkCore;
using SimpleAgenda.Aid.ExtensionClasses;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Context
{
    internal class SqliteContext : DbContext, IContext
    {
        private string _connectionString;
        private string _defaultConnectionString = "Data Source=agenda.db";

        /// <summary>
        /// May throws an ArgumentException if the connection string is null or empty.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="ArgumentException">If connetion string in null or empety</exception>
        public SqliteContext(string? connectionString=null)
        {
            _connectionString = connectionString?.NullOrEmptyValidator() ?? _defaultConnectionString ;
        }

        public DbSet<ScheduleDto> Schedules { get; set; }  // DbSet para ScheduleDto
        public DbSet<EventDto> Events { get; set; }  // DbSet para EventDto
        public DbSet<AppointmentDto> Appointments { get; set; }  // DbSet para AppointmentDto
        public DbSet<LocationDto> Locations { get; set; }  // DbSet para LocationDto

        /// <summary>
        /// Ensures that the database is migrated to the latest version.
        /// </summary>
        public void EnsureMigrated() => Database.Migrate();
        
        /// <summary>
        /// Ensures that the database is created if it does not already exist.
        /// </summary>
        public void EnsureCreated() => Database.EnsureCreated();
        
        /// <summary>
        /// Ensures that the database is deleted.
        /// </summary>
        public void EnsureDeleted() => Database.EnsureDeleted();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring composite key for LocationDto
            modelBuilder.Entity<LocationDto>()
                .HasIndex(l => new { l.Street, l.Number, l.City, l.PostalCode, l.Country, l.State, l.Complement })
                .IsUnique(true);

            // Configuring relationship for EventDto and LocationDto as one-to-one, with LocationDto being optional
            modelBuilder.Entity<EventDto>()
                .HasOne(e => e.Location)
                .WithOne()
                .HasForeignKey<EventDto>("location_id")
                .IsRequired(false);

            // Relation 1:1 between AppointmentDto and EventDto
            modelBuilder.Entity<AppointmentDto>()
                .HasOne(a => a.Event)
                .WithOne()
                .HasForeignKey<AppointmentDto>("event_id")
                .IsRequired(true);

            // Relation 1:1 between AppointmentDto and ScheduleDto
            modelBuilder.Entity<AppointmentDto>()
                .HasOne< ScheduleDto>()
                .WithMany()
                .HasForeignKey("schedule_id")
                .IsRequired(true);

            // Configuring ScheduleDto
            modelBuilder.Entity<ScheduleDto>()
                .HasMany(s => s.PendingAppointments)
                .WithOne();

        }

    }
}
