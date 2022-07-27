namespace InventoryMgmt.Server.Models
{
    public class InventoryWarehouseItem
    {
        public int Id { get; set; }

        public int InventoryId { get; set; }

        public Inventory? Inventory { get; set; }

        public int WarehouseItemId { get; set; }

        public string? StorageLocation { get; set; }

        public WarehouseItem? WarehouseItem { get; set; }

        public InventorySerial? InventorySerial { get; set; }
    }
}
