using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UCP_HUANCAYO.Models;

namespace UCP_HUANCAYO.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Predio> Predios { get; set; }
        public DbSet<PredioTipo> PredioTipos { get; set; }
        public DbSet<PredioImagen> PredioImagenes { get; set; }
        public DbSet<Administrado> Administrados { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<CronogramaPago> CronogramaPagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Predio>()
                .HasOne(p => p.PredioTipo)
                .WithMany(pt => pt.Predios)
                .HasForeignKey(p => p.IdPredioTipo);

            modelBuilder.Entity<PredioImagen>()
                .HasOne(pi => pi.Predio)
                .WithMany(p => p.Imagenes)
                .HasForeignKey(pi => pi.IdPredio);

            modelBuilder.Entity<Contrato>()
                .HasMany(c => c.CronogramasPago)
                .WithOne(cp => cp.Contrato)
                .HasForeignKey(cp => cp.IdContrato);

            modelBuilder.Entity<Contrato>()
                .HasOne(c => c.Administrado)
                .WithMany()
                .HasForeignKey(c => c.IdAdministrado);

            modelBuilder.Entity<Contrato>()
                .HasOne(c => c.Predio)
                .WithMany()
                .HasForeignKey(c => c.IdPredio);

            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Predio)
                .WithMany()
                .HasForeignKey(a => a.IdPredio);

            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Administrado)
                .WithMany()
                .HasForeignKey(a => a.IdAdministrado);
        }

    }
}
