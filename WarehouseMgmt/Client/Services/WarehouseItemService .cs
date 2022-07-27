using System.Net.Http.Json;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Services
{
    public class WarehouseItemService : IWarehouseItemService
    {
        private readonly HttpClient httpClient;

        public WarehouseItemService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> CreateWarehouseItem(WarehouseItemDto warehouseItemDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/warehouseItems", warehouseItemDto);
                var content = await response.Content.ReadFromJsonAsync<WarehouseDto>();
                response.EnsureSuccessStatusCode();

                if (content == null)
                    throw new Exception("Warehouse Item information was not returned!");

                return content.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteWarehouseItem(int warehouseItemId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/warehouseItems/{warehouseItemId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WarehouseItemDto> GetWarehouseItem(int warehouseItemId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<WarehouseItemDto>($"api/warehouseItems/{warehouseItemId}");

                if (response == null)
                    throw new Exception("Warehouse Item information was not found!");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ShipItems(int warehouseId, List<int> warehouseItemIds)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/warehouseItems/shipItems/{warehouseId}", warehouseItemIds);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateWarehouseItem(WarehouseItemDto warehouseItemDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/warehouseItems/{warehouseItemDto.Id}", warehouseItemDto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
