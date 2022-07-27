namespace InventoryMgmt.Server.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public Warehouse? Warehouse { get; set; }

        public string? PartNumber { get; set; }

        public string? Description { get; set; }

        public int Qty { get; set; }

        public List<InventoryWarehouseItem>? InventoryWarehouseItems { get; set; }
    }
}
