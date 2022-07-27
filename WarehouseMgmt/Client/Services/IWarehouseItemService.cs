using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Services
{
    public interface IWarehouseItemService
    {
        Task DeleteWarehouseItem(int warehouseItemId);
        Task<int> CreateWarehouseItem(WarehouseItemDto warehouseItemDto);
        Task<WarehouseItemDto> GetWarehouseItem(int warehouseItemId);
        Task UpdateWarehouseItem(WarehouseItemDto warehouseItemDto);
        Task ShipItems(int warehouseId, List<int> warehouseItemIds);
    }
}
