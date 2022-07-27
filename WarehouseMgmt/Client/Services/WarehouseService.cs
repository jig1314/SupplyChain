using System.Net.Http.Json;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly HttpClient httpClient;

        public WarehouseService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int> CreateWarehouse(WarehouseDto warehouseDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"api/warehouses", warehouseDto);
                var content = await response.Content.ReadFromJsonAsync<WarehouseDto>();
                response.EnsureSuccessStatusCode();

                if (content == null)
                    throw new Exception("Warehouse information was not returned!");

                return content.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteWarehouse(int warehouseId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/warehouses/{warehouseId}");
                response.EnsureSuccessStatusCode();
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
                    throw new Exception("Warehouse information was not found!");

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

        public async Task<List<WarehouseItemDto>> GetWarehouseItems(int warehouseId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<WarehouseItemDto>>($"api/warehouses/getWarehouseItems/{warehouseId}");

                if (response == null)
                    throw new Exception("No warehouse items were found for this warehouse!");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateWarehouse(WarehouseDto warehouseDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/warehouses/{warehouseDto.Id}", warehouseDto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
