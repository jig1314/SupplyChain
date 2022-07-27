using AutoMapper;
using BlazorStrap;
using BlazorStrap.Extensions.FluentValidation;
using Microsoft.AspNetCore.Components;
using WarehouseMgmt.Client.Components.Modals;
using WarehouseMgmt.Client.Services;
using WarehouseMgmt.Client.ViewModels;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Pages
{
    public partial class WarehouseItem : ComponentBase
    {
        public enum ViewMode
        {
            Add,
            Edit
        }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IWarehouseService? WarehouseService { get; set; }

        [Inject]
        public IWarehouseItemService? WarehouseItemService { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }

        [Parameter]
        public ViewMode CurrentViewMode { get; set; }

        [Parameter]
        public string? WarehouseId { get; set; }

        private int? warehouseId;
        private WarehouseDto? warehouseDto;

        [Parameter]
        public string? WarehouseItemId { get; set; }

        private int? warehouseItemId;
        private WarehouseItemDto? warehouseItemDto;

        public WarehouseItemViewModel WarehouseItemViewModel { get; set; } = new WarehouseItemViewModel();

        protected DeleteWarehouseItemModal? DeleteWarehouseItemModal { get; set; }

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(WarehouseId))
            {
                warehouseId = Convert.ToInt32(WarehouseId);
                CurrentViewMode = ViewMode.Add;
                await GetWarehouseInformation();
            }

            if (!string.IsNullOrWhiteSpace(WarehouseItemId))
            {
                warehouseItemId = Convert.ToInt32(WarehouseItemId);
                CurrentViewMode = ViewMode.Edit;
                await GetWarehouseItemInformation();
            }

            StateHasChanged();
        }

        private async Task GetWarehouseInformation()
        {
            if (WarehouseService == null || Mapper == null || !warehouseId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                warehouseDto = await WarehouseService.GetWarehouse(warehouseId.Value);
                WarehouseItemViewModel.WarehouseName = warehouseDto.Name;
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

        private async Task GetWarehouseItemInformation()
        {
            if (WarehouseItemService == null || Mapper == null || !warehouseItemId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                warehouseItemDto = await WarehouseItemService.GetWarehouseItem(warehouseItemId.Value);
                WarehouseItemViewModel = Mapper.Map<WarehouseItemViewModel>(warehouseItemDto);
                warehouseId = warehouseItemDto.WarehouseId;
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

        protected void HasSerialNumberChanged(bool hasSerialNumber)
        {
            WarehouseItemViewModel.HasSerialNumber = hasSerialNumber;
            if (hasSerialNumber)
            {
                WarehouseItemViewModel.Qty = "1";
            }
            else
            {
                WarehouseItemViewModel.SerialNumber = "";
            }

            StateHasChanged();
        }

        protected async Task SubmitWarehouseItem()
        {
            if (CurrentViewMode == ViewMode.Add)
            {
                await CreateWarehouseItem();
            }
            else
            {
                await UpdateWarehouseItem();
            }
        }

        private async Task CreateWarehouseItem()
        {
            if (WarehouseItemService == null || Mapper == null || NavigationManager == null || !warehouseId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                var warehouseItemDto = Mapper.Map<WarehouseItemDto>(WarehouseItemViewModel);
                warehouseItemDto.WarehouseId = warehouseId.Value;

                await WarehouseItemService.CreateWarehouseItem(warehouseItemDto);

                GoBackToWarehousePage();
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

        private async Task UpdateWarehouseItem()
        {
            if (WarehouseItemService == null || Mapper == null || !warehouseId.HasValue || !warehouseItemId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                var warehouseItemDto = Mapper.Map<WarehouseItemDto>(WarehouseItemViewModel);
                warehouseItemDto.Id = warehouseItemId.Value;
                warehouseItemDto.WarehouseId = warehouseId.Value;

                await WarehouseItemService.UpdateWarehouseItem(warehouseItemDto);
                GoBackToWarehousePage();
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

        protected void GoBackToWarehousePage()
        {
            if (NavigationManager == null || !warehouseId.HasValue)
                return;

            NavigationManager.NavigateTo($"/warehouse/{warehouseId.Value}");
        }

        protected void OpenDeleteWarehouseItemModal()
        {
            if (DeleteWarehouseItemModal == null || !warehouseItemId.HasValue)
                return;

            DeleteWarehouseItemModal.Show(warehouseItemId.Value);
        }

        protected async Task DeleteWarehouseItem(int warehouseItemId)
        {
            if (WarehouseItemService == null || WarehouseService == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                await WarehouseItemService.DeleteWarehouseItem(warehouseItemId);

                GoBackToWarehousePage();
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
