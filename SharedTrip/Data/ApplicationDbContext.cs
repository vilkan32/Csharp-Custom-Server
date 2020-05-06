namespace SharedTrip
{
    using Microsoft.EntityFrameworkCore;
    using SharedTrip.Models;
    using System;

    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>()
                .HasKey(ut => new { ut.TripId, ut.UserId });

            modelBuilder.Entity<UserTrip>()
       .HasOne(bc => bc.User)
       .WithMany(b => b.UserTrips)
       .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserTrip>()
                .HasOne(bc => bc.Trip)
                .WithMany(c => c.UserTrips)
                .HasForeignKey(bc => bc.TripId);
        }

        internal object FirstOrDefault()
        {
            throw new NotImplementedException();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }
    }
}
