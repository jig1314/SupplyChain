namespace InventoryMgmt.Shared.DTOs
{
    public class WarehouseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string StreetAddress { get; set; } = "";

        public string City { get; set; } = "";

        public string State { get; set; } = "";

        public string ZipCode { get; set; } = "";

        public string Country { get; set; } = "";
    }
}
