using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.Entities;

namespace WebApi.Data
{
    public class ImsDbContext : DbContext, IImsDbContext
    {
        public ImsDbContext(DbContextOptions<ImsDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Dues> Dues { get; set; }
        public DbSet<Housing> Housings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Housing>().Property(h => h.PlanType).HasConversion<string>();
            modelBuilder.Entity<Housing>().Property(h => h.ApartmentStatus).HasConversion<string>();
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();

            // User table dependent, Housing table principal
            modelBuilder.Entity<User>()
                .HasOne(u => u.Housing)      // User → navigation property
                .WithOne(h => h.User)        // Housing → navigation property
                .HasForeignKey<User>(u => u.HousingId);  // FK User.HousingId

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Housing)
                .WithMany(h => h.Invoices)
                .HasForeignKey(i => i.HousingId);
                
            modelBuilder.Entity<Invoice>()
                .Property(i => i.PaymentStatus)
                .HasDefaultValue(PaymentStatus.NotPaid);
        }

    }

}