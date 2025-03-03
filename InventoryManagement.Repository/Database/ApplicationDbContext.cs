using InventoryManagement.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Repository.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Booking>()
                .HasOne(o => o.Member)
                .WithMany()
                .HasForeignKey(o => o.MemberId);

            modelBuilder.Entity<Booking>()
                .HasOne(o => o.Inventory)
                .WithMany()
                .HasForeignKey(o => o.InventoryId);
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
