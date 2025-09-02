using Microsoft.EntityFrameworkCore;
using DTI.Domain; // <- Importante, para enxergar a classe Lead

namespace DTI.Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        { }

        public DbSet<Lead> Leads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lead>(entity =>
            {
                entity.ToTable("Leads");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ContactFirstName).HasMaxLength(100);
                entity.Property(e => e.ContactLastName).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.Suburb).HasMaxLength(100);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
