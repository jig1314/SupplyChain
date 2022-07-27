using Microsoft.AspNetCore.Components;
using WarehouseMgmt.Client.Components.Modals;
using WarehouseMgmt.Client.Services;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Pages
{
    public partial class Warehouses : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IWarehouseService? WarehouseService { get; set; }

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        protected List<WarehouseDto> WarehouseDtos { get; set; } = new List<WarehouseDto>();

        protected DeleteWarehouseModal? DeleteWarehouseModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetWarehouses();
        }

        private async Task GetWarehouses()
        {
            if (WarehouseService == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                WarehouseDtos = await WarehouseService.GetWarehouses();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message}";
            }
            finally
            {
                LoadingData = false;
            }
        }

        protected void AddNewWarehouse()
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo("/warehouse/create");
        }

        protected void EditWarehouse(int warehouseId)
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo($"/warehouse/edit/{warehouseId}");
        }

        protected void ViewWarehouse(int warehouseId)
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo($"/warehouse/{warehouseId}");
        }

        protected void OpenDeleteWarehouseModal(int warehouseId)
        {
            if (DeleteWarehouseModal == null)
                return;

            DeleteWarehouseModal.Show(warehouseId);
        }

        protected async Task DeleteWarehouse(int warehouseId)
        {
            if (WarehouseService == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                await WarehouseService.DeleteWarehouse(warehouseId);
                WarehouseDtos = await WarehouseService.GetWarehouses();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message}";
            }
            finally
            {
                LoadingData = false;
            }
        }
    }
}
