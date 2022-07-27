using InventoryMgmt.Client.Services;
using InventoryMgmt.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace InventoryMgmt.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public IInventoryService? InventoryService { get; set; }

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        protected List<WarehouseDto> WarehouseDtos { get; set; } = new List<WarehouseDto>();

        protected override async Task OnInitializedAsync()
        {
            if (InventoryService == null)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                WarehouseDtos = await InventoryService.GetWarehouses();
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
