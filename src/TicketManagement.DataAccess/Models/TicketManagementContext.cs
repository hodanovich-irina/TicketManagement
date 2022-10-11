using Microsoft.EntityFrameworkCore;

namespace TicketManagement.DataAccess.Models
{
    /// <summary>
    /// Class context of datas.
    /// </summary>
    public class TicketManagementContext : DbContext
    {
        public TicketManagementContext(DbContextOptions<TicketManagementContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().Ignore(eventProperty => eventProperty.BaseAreaPrice);
        }

        public DbSet<Area> Area { get; set; }

        public DbSet<Event> Event { get; set; }

        public DbSet<EventArea> EventArea { get; set; }

        public DbSet<EventSeat> EventSeat { get; set; }

        public DbSet<Layout> Layout { get; set; }

        public DbSet<Seat> Seat { get; set; }

        public DbSet<Venue> Venue { get; set; }

        public DbSet<Ticket> Ticket { get; set; }
    }
}
