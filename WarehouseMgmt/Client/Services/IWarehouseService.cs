using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Services
{
    public interface IWarehouseService
    {
        Task<List<WarehouseDto>> GetWarehouses();
        Task DeleteWarehouse(int warehouseId);
        Task<int> CreateWarehouse(WarehouseDto warehouseDto);
        Task<WarehouseDto> GetWarehouse(int warehouseId);
        Task UpdateWarehouse(WarehouseDto warehouseDto);
        Task<List<WarehouseItemDto>> GetWarehouseItems(int warehouseId);
    }
}
