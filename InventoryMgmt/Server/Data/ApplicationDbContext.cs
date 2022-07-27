using InventoryMgmt.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InventoryMgmt.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            DbContextOptions options) : base(options) { }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Inventory> Inventory { get; set; }

        public DbSet<InventorySerial> InventorySerials { get; set; }

        public DbSet<InventoryWarehouseItem> InventoryWarehouseItems { get; set; }

        public DbSet<WarehouseItem> WarehouseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Warehouse>()
                .HasKey(w => w.Id)
                .HasName("PrimaryKey_WarehouseId");

            builder.Entity<Warehouse>().Property(w => w.WarehouseId).IsRequired();
            builder.Entity<Warehouse>().Property(w => w.Name).IsRequired();
            builder.Entity<Warehouse>().Property(w => w.StreetAddress).IsRequired();
            builder.Entity<Warehouse>().Property(w => w.City).IsRequired();
            builder.Entity<Warehouse>().Property(w => w.State).IsRequired();
            builder.Entity<Warehouse>().Property(w => w.ZipCode).IsRequired();
            builder.Entity<Warehouse>().Property(w => w.Country).IsRequired();

            builder.Entity<WarehouseItem>()
                .HasKey(w => w.Id)
                .HasName("PrimaryKey_WarehouseItemId");

            builder.Entity<WarehouseItem>()
                .HasOne(i => i.Warehouse)
                .WithMany(w => w.WarehouseItems)
                .IsRequired()
                .HasForeignKey(i => i.WarehouseId)
                .HasConstraintName("ForeignKey_WarehouseItem_Warehouse");

            builder.Entity<WarehouseItem>().Property(w => w.WarehouseItemId).IsRequired();
            builder.Entity<WarehouseItem>().Property(w => w.StorageLocation).IsRequired();
            builder.Entity<WarehouseItem>().Property(w => w.PartNumber).IsRequired();
            builder.Entity<WarehouseItem>().Property(w => w.Description).IsRequired();
            builder.Entity<WarehouseItem>().Property(w => w.SerialNumber).IsRequired();
            builder.Entity<WarehouseItem>().Property(w => w.Qty).IsRequired();

            builder.Entity<Inventory>()
                .HasKey(i => i.Id)
                .HasName("PrimaryKey_InventoryId");

            builder.Entity<Inventory>()
                .HasOne(i => i.Warehouse)
                .WithMany(w => w.Inventory)
                .IsRequired()
                .HasForeignKey(i => i.WarehouseId)
                .HasConstraintName("ForeignKey_Inventory_Warehouse");

            builder.Entity<Inventory>().Property(i => i.PartNumber).IsRequired();
            builder.Entity<Inventory>().Property(i => i.Description).IsRequired();
            builder.Entity<Inventory>().Property(i => i.Qty).IsRequired();

            builder.Entity<InventoryWarehouseItem>()
                .HasKey(i => i.Id)
                .HasName("PrimaryKey_InventoryWarehouseItemId");

            builder.Entity<InventoryWarehouseItem>()
                .HasOne(i => i.Inventory)
                .WithMany(w => w.InventoryWarehouseItems)
                .IsRequired()
                .HasForeignKey(i => i.InventoryId)
                .HasConstraintName("ForeignKey_InventoryWarehouseItem_Inventory");

            builder.Entity<InventoryWarehouseItem>()
                .HasOne(i => i.WarehouseItem)
                .WithOne(w => w.InventoryWarehouseItem)
                .IsRequired()
                .HasForeignKey<InventoryWarehouseItem>(i => i.WarehouseItemId)
                .HasConstraintName("ForeignKey_InventoryWarehouseItem_WarehouseItem")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<InventoryWarehouseItem>().Property(i => i.StorageLocation).IsRequired();

            builder.Entity<InventorySerial>()
                .HasKey(i => i.InventoryWarehouseItemId)
                .HasName("PrimaryKey_InventorySerial_InventoryWarehouseItemId");

            builder.Entity<InventorySerial>()
                .HasOne(i => i.InventoryWarehouseItem)
                .WithOne(w => w.InventorySerial)
                .IsRequired()
                .HasForeignKey<InventorySerial>(i => i.InventoryWarehouseItemId)
                .HasConstraintName("ForeignKey_InventorySerial_InventoryWarehouseItemId");

            builder.Entity<InventorySerial>().Property(i => i.SerialNumber).IsRequired();
        }
    }
}