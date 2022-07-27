namespace InventoryMgmt.Server.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public string? Name { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }

        public List<WarehouseItem>? WarehouseItems { get; set; }

        public List<Inventory>? Inventory { get; set; }
    }
}
