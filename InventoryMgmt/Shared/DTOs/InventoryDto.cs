namespace InventoryMgmt.Shared.DTOs
{
    public class InventoryDto
    {
        public int Id { get; set; }

        public string? PartNumber { get; set; }

        public string? Description { get; set; }

        public int Qty { get; set; }
    }
}
