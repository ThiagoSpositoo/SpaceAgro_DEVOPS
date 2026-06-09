using Microsoft.EntityFrameworkCore;
using SpaceAgro.DotNetApi.Models;

namespace SpaceAgro.DotNetApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Talhao> Talhoes { get; set; }
        public DbSet<LeituraSensor> LeiturasSensores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Talhao>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd();
                entity.Property(t => t.Nome).IsRequired();
                entity.Property(t => t.Cultura).IsRequired();
            });

            modelBuilder.Entity<LeituraSensor>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Id).ValueGeneratedOnAdd();

                entity.HasOne(l => l.Talhao)
                    .WithMany(t => t.Leituras)
                    .HasForeignKey(l => l.IdDispositivo)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
