using Microsoft.EntityFrameworkCore;
using System;

namespace Api.Database
{
    public class MaquillajeDbContext: DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        public MaquillajeDbContext(DbContextOptions<MaquillajeDbContext> options)
            : base(options)
        {
        }

        // Opcional: configuraciones adicionales con Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Clientes"); // nombre de la tabla en la BD
        }
    }
}
