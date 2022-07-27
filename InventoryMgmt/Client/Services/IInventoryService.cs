using InventoryMgmt.Shared.DTOs;

namespace InventoryMgmt.Client.Services
{
    public interface IInventoryService
    {
        Task<List<WarehouseDto>> GetWarehouses();
        Task<List<InventoryDto>> GetInventory(int warehouseId);
        Task<WarehouseDto> GetWarehouse(int warehouseId);
        Task<List<InventoryDistributionDto>> GetInventoryDistribution(int inventoryId);
    }
}
