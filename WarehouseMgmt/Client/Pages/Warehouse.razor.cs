using AutoMapper;
using Microsoft.AspNetCore.Components;
using WarehouseMgmt.Client.Components.Modals;
using WarehouseMgmt.Client.Services;
using WarehouseMgmt.Client.ViewModels;
using WarehouseMgmt.Shared.DTOs;

namespace WarehouseMgmt.Client.Pages
{
    public partial class Warehouse : ComponentBase
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
        public IMapper? Mapper { get; set; }

        [Parameter]
        public ViewMode CurrentViewMode { get; set; }

        [Parameter]
        public string? WarehouseId { get; set; }

        private int? _warehouseId;

        protected DeleteWarehouseModal? DeleteWarehouseModal { get; set; }

        public WarehouseViewModel WarehouseViewModel { get; set; } = new WarehouseViewModel();

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            CurrentViewMode = string.IsNullOrWhiteSpace(WarehouseId) ? ViewMode.Add : ViewMode.Edit;

            if (CurrentViewMode == ViewMode.Edit)
            {
                _warehouseId = Convert.ToInt32(WarehouseId);
                await GetWarehouseInformation();
            }
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

        protected async Task SubmitWarehouse()
        {
            if (CurrentViewMode == ViewMode.Add)
            {
                await CreateWarehouse();
            }
            else
            {
                await UpdateWarehouse();
            }
        }
        private async Task CreateWarehouse()
        {
            if (WarehouseService == null || Mapper == null || NavigationManager == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                var warehouseDto = Mapper.Map<WarehouseDto>(WarehouseViewModel);

                var warehouseId = await WarehouseService.CreateWarehouse(warehouseDto);

                CurrentViewMode = ViewMode.Edit;
                _warehouseId = warehouseId;

                GoToWarehousePage();
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

        private async Task UpdateWarehouse()
        {
            if (WarehouseService == null || Mapper == null || !_warehouseId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                var warehouseDto = Mapper.Map<WarehouseDto>(WarehouseViewModel);
                warehouseDto.Id = _warehouseId.Value;

                await WarehouseService.UpdateWarehouse(warehouseDto);
                await GetWarehouseInformation();
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

        protected void OpenDeleteWarehouseModal()
        {
            if (DeleteWarehouseModal == null || !_warehouseId.HasValue)
                return;

            DeleteWarehouseModal.Show(_warehouseId.Value);
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
                GoBackToWarehousesPage();
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

        protected void GoToWarehousePage()
        {
            if (NavigationManager == null || !_warehouseId.HasValue)
                return;

            NavigationManager.NavigateTo($"/warehouse/{_warehouseId.Value}");
        }

        protected void GoBackToWarehousesPage()
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo("/manageWarehouses/");
        }
    }
}
