using Microsoft.EntityFrameworkCore;
using WarehouseMgmt.Server.Models;

namespace WarehouseMgmt.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            DbContextOptions options) : base(options) { }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<WarehouseItem> WarehouseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Warehouse>()
                .HasKey(w => w.Id)
                .HasName("PrimaryKey_WarehouseId");

            builder.Entity<Warehouse>().Property(m => m.Name).IsRequired();
            builder.Entity<Warehouse>().Property(m => m.StreetAddress).IsRequired();
            builder.Entity<Warehouse>().Property(m => m.City).IsRequired();
            builder.Entity<Warehouse>().Property(m => m.State).IsRequired();
            builder.Entity<Warehouse>().Property(m => m.ZipCode).IsRequired();
            builder.Entity<Warehouse>().Property(m => m.Country).IsRequired();

            builder.Entity<WarehouseItem>()
                .HasKey(w => w.Id)
                .HasName("PrimaryKey_WarehouseItemId");

            builder.Entity<WarehouseItem>()
                .HasOne(e => e.Warehouse)
                .WithMany(u => u.Items)
                .IsRequired()
                .HasForeignKey(w => w.WarehouseId)
                .HasConstraintName("ForeignKey_WarehouseItem_Warehouse");

            builder.Entity<WarehouseItem>().Property(m => m.StorageLocation).IsRequired();
            builder.Entity<WarehouseItem>().Property(m => m.PartNumber).IsRequired();
            builder.Entity<WarehouseItem>().Property(m => m.Description).IsRequired();
            builder.Entity<WarehouseItem>().Property(m => m.SerialNumber).IsRequired();
            builder.Entity<WarehouseItem>().Property(m => m.Qty).IsRequired();
        }
    }
}