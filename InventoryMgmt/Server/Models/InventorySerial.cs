namespace InventoryMgmt.Server.Models
{
    public class InventorySerial
    {
        public int InventoryWarehouseItemId { get; set; }

        public InventoryWarehouseItem? InventoryWarehouseItem { get; set; }

        public string? SerialNumber { get; set; }
    }
}
