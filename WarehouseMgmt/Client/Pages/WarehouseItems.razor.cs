using AutoMapper;
using Microsoft.AspNetCore.Components;
using WarehouseMgmt.Client.Components.Modals;
using WarehouseMgmt.Client.Services;
using WarehouseMgmt.Client.ViewModels;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Pages
{
    public partial class WarehouseItems : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IWarehouseService? WarehouseService { get; set; }

        [Inject]
        public IWarehouseItemService? WarehouseItemService { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }

        [Parameter]
        public string? WarehouseId { get; set; }

        private int? _warehouseId;

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        public WarehouseViewModel WarehouseViewModel { get; set; } = new WarehouseViewModel();

        public List<WarehouseItemDto> WarehouseItemDtos { get; set; } = new List<WarehouseItemDto>();

        public List<int> SelectedItemIds { get; set; } = new List<int>();

        protected DeleteWarehouseItemModal? DeleteWarehouseItemModal { get; set; }

        protected ShipItemsModal? ShipItemsModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _warehouseId = Convert.ToInt32(WarehouseId);
            await GetWarehouseInformation();
        }

        private async Task GetWarehouseInformation()
        {
            if (WarehouseService == null || Mapper == null || !_warehouseId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                var warehouseDto = await WarehouseService.GetWarehouse(_warehouseId.Value);
                WarehouseViewModel = Mapper.Map<WarehouseViewModel>(warehouseDto);

                WarehouseItemDtos = await WarehouseService.GetWarehouseItems(_warehouseId.Value);
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

        protected void AddNewWarehouseItem()
        {
            if (NavigationManager == null || !_warehouseId.HasValue)
                return;

            NavigationManager.NavigateTo($"/warehouse/receive/{_warehouseId.Value}");
        }

        protected void EditWarehouseItem(int warehouseItemId)
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo($"/warehouseItem/edit/{warehouseItemId}");
        }

        protected void OpenDeleteWarehouseItemModal(int warehouseItemId)
        {
            if (DeleteWarehouseItemModal == null)
                return;

            DeleteWarehouseItemModal.Show(warehouseItemId);
        }

        protected async Task DeleteWarehouseItem(int warehouseItemId)
        {
            if (WarehouseItemService == null || WarehouseService == null || !_warehouseId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                await WarehouseItemService.DeleteWarehouseItem(warehouseItemId);

                WarehouseItemDtos = await WarehouseService.GetWarehouseItems(_warehouseId.Value);
                StateHasChanged();
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

        protected void SelectedItemsChanged(int warehouseItemId, bool isChecked)
        {
            if (!isChecked && SelectedItemIds.Contains(warehouseItemId))
            {
                SelectedItemIds.Remove(warehouseItemId);
            }
            else if (isChecked && !SelectedItemIds.Contains(warehouseItemId))
            {
                SelectedItemIds.Add(warehouseItemId);
            }
        }

        protected void GoBackToWarehousesPage()
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo("/manageWarehouses/");

        }

        protected void OpenShipItemModal()
        {
            if (ShipItemsModal == null || !_warehouseId.HasValue)
                return;

            ShipItemsModal.Show(_warehouseId.Value);
        }

        protected async Task OnItemsShipped(int shipToWarehouseId)
        {
            if (WarehouseService == null || WarehouseItemService == null || !_warehouseId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                await WarehouseItemService.ShipItems(shipToWarehouseId, SelectedItemIds);

                WarehouseItemDtos = await WarehouseService.GetWarehouseItems(_warehouseId.Value);
                StateHasChanged();
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
