using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class SpiritualDbContext:IdentityDbContext<Users>
    {
        public SpiritualDbContext(DbContextOptions<SpiritualDbContext> options)
           : base(options)
        {
        }

        public DbSet<Planes> Planes { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Obras> Obras { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Planes>().ToTable("Planes");
            builder.Entity<Categorias>().ToTable("Categorias");
            builder.Entity<Obras>().ToTable("Obras");

            base.OnModelCreating(builder);
        }

    }
}
