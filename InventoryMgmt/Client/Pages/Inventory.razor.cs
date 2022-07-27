using InventoryMgmt.Client.Components.Modals;
using InventoryMgmt.Client.Services;
using InventoryMgmt.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace InventoryMgmt.Client.Pages
{
    public partial class Inventory : ComponentBase
    {
        [Inject]
        public IInventoryService? InventoryService { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        [Parameter]
        public string? WarehouseId { get; set; }

        private int? _warehouseId;

        protected WarehouseDto Warehouse { get; set; } = new WarehouseDto();

        protected List<InventoryDto> InventoryDtos { get; set; } = new List<InventoryDto>();

        protected ViewInventoryModal? ViewInventoryModal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (InventoryService == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;

                _warehouseId = Convert.ToInt32(WarehouseId);

                if (!_warehouseId.HasValue)
                    throw new Exception("No warehouse specified!");

                Warehouse = await InventoryService.GetWarehouse(_warehouseId.Value);

                InventoryDtos = await InventoryService.GetInventory(_warehouseId.Value);
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

        protected void GoBackToWarehousesPage()
        {
            if (NavigationManager == null)
                return;

            NavigationManager.NavigateTo("/warehouses/");

        }

        protected void OpenInventoryModal(int inventoryId)
        {
            if (ViewInventoryModal == null)
                return;

            ViewInventoryModal.Show(inventoryId);
        }
    }
}
