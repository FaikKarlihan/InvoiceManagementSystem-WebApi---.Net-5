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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Housing>().Property(h => h.PlanType).HasConversion<string>();
            modelBuilder.Entity<Housing>().Property(h => h.ApartmentStatus).HasConversion<string>();
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<Invoice>().Property(u => u.PaymentStatus).HasConversion<string>();
            modelBuilder.Entity<Invoice>().Property(u => u.Type).HasConversion<string>();

            // Invoice (fatura) tablosu ile Housing (konut) tablosu arasında ilişki tanımlanıyor
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Housing)             // Bir faturanın bir konuta ait olduğunu belirtir (Invoice -> Housing)
                .WithMany(h => h.Invoices)          // Bir konutun birden fazla faturası olabileceğini belirtir (Housing -> Invoices)
                .HasForeignKey(i => i.HousingId);   // Fatura tablosundaki Foreign Key'in HousingId olduğunu belirtir


            // Invoice tablosundaki PaymentStatus özelliği için varsayılan değer atanıyor
            modelBuilder.Entity<Invoice>()
                .Property(i => i.PaymentStatus)
                .HasDefaultValue(PaymentStatus.NotPaid); // Eğer PaymentStatus set edilmezse, varsayılan olarak NotPaid olur


            // Housing (konut) tablosu ile User (kullanıcı) tablosu arasında birebir ilişki tanımlanıyor
            modelBuilder.Entity<Housing>()
                .HasOne(h => h.User)                    // Her konut bir kullanıcıya ait olabilir
                .WithOne(u => u.Housing)                // Her kullanıcının bir konutu olabilir
                .HasForeignKey<Housing>(h => h.UserId)  // Foreign Key konut tablosunda, UserId alanı
                .OnDelete(DeleteBehavior.SetNull);      // Eğer kullanıcı silinirse, Housing tablosundaki UserId alanı null yapılır

        }

    }

}