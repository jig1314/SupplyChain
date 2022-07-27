namespace InventoryMgmt.Shared.DTOs
{
    public class InventoryDistributionDto
    {
        public string? StorageLocation { get; set; }

        public string? PartNumber { get; set; }

        public string? Description { get; set; }

        public int? Qty { get; set; }

        public string? SerialNumber { get; set; }
    }
}
