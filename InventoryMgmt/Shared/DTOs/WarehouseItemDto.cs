namespace InventoryMgmt.Shared.DTOs
{
    public class WarehouseItemDto
    {
        public int Id { get; set; }

        public int WarehouseId { get; set; }

        public string? StorageLocation { get; set; }

        public string? PartNumber { get; set; }

        public string? Description { get; set; }

        public string? SerialNumber { get; set; }

        public int Qty { get; set; }
    }
}
