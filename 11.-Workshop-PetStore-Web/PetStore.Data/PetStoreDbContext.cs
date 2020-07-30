using Microsoft.EntityFrameworkCore;
using PetStore.Comman;
using PetStore.Models;
using System;

namespace PetStore.Data
{
    public class PetStoreDbContext : DbContext
    {
        public PetStoreDbContext()
        {

        }
        public PetStoreDbContext(DbContextOptions options)
        : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConfiguration.defaultConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetStoreDbContext).Assembly);

            //modelBuilder.Entity<ClientProduct>(entity =>
            //{
            //    entity.HasKey(x => new { x.ClientId, x.ProductId });
            //});

            //modelBuilder.Entity<Order>(entity =>
            //{
            //    entity.Ignore(x => x.TotalPrice);
            //});
        }

        public virtual DbSet<Breed> Breeds { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientProduct> ClientProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Product> Products { get; set; }

    }
}
