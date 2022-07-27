using InventoryMgmt.Shared.DTOs;
using System.Net.Http.Json;

namespace InventoryMgmt.Client.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient httpClient;

        public InventoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<InventoryDto>> GetInventory(int warehouseId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<InventoryDto>>($"api/warehouses/getInventory/{warehouseId}");

                if (response == null)
                    throw new Exception("No inventory found!");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WarehouseDto> GetWarehouse(int warehouseId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<WarehouseDto>($"api/warehouses/{warehouseId}");

                if (response == null)
                    throw new Exception("Warehouse was not found!");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WarehouseDto>> GetWarehouses()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<WarehouseDto>>($"api/warehouses");

                if (response == null)
                    throw new Exception("No warehouses were found!");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
