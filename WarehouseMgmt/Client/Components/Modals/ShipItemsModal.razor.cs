using BlazorStrap;
using Microsoft.AspNetCore.Components;
using WarehouseMgmt.Client.Services;
using WarehouseMgmt.Client.ViewModels;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Components.Modals
{
    public partial class ShipItemsModal : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IWarehouseService? WarehouseService { get; set; }

        [Inject]
        public IWarehouseItemService? WarehouseItemService { get; set; }

        private int _warehouseId;

        protected BSModal? Modal { get; set; }

        [Parameter]
        public EventCallback<int> OnShipItems { get; set; }

        public ShipItemViewModel ShipItemViewModel { get; set; } = new ShipItemViewModel();

        public List<WarehouseDto> Warehouses { get; set; } = new List<WarehouseDto>();

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        private async Task RefreshWarehouses()
        {
            if (WarehouseService == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                var warehouses = await WarehouseService.GetWarehouses();
                Warehouses = warehouses.Where(w => w.Id != _warehouseId).ToList();
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

        protected async void ShipItems()
        {
            if (Modal == null || !ShipItemViewModel.IdWarehouse.HasValue)
                return;

            await Modal.HideAsync();

            await OnShipItems.InvokeAsync(ShipItemViewModel.IdWarehouse.Value);
        }

        public async void Show(int warehouseId)
        {
            if (Modal == null)
                return;

            _warehouseId = warehouseId;

            await Modal.ShowAsync();

            await RefreshWarehouses();
        }

        public void Hide()
        {
            if (Modal == null)
                return;

            Modal.HideAsync();
        }
    }
}
