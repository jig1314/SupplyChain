using BlazorStrap;
using InventoryMgmt.Client.Services;
using InventoryMgmt.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace InventoryMgmt.Client.Components.Modals
{
    public partial class ViewInventoryModal : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IInventoryService? InventoryService { get; set; }

        private int? _inventoryId;

        protected BSModal? Modal { get; set; }

        public string ErrorMessage { get; set; } = "";

        public bool LoadingData { get; set; } = false;

        public List<InventoryDistributionDto> InventoryDistribution { get; set; } = new List<InventoryDistributionDto>();

        public async void Show(int inventoryId)
        {
            if (Modal == null)
                return;

            _inventoryId = inventoryId;

            await Modal.ShowAsync();

            await RefreshInventoryData();
        }

        private async Task RefreshInventoryData()
        {
            if (InventoryService == null || !_inventoryId.HasValue)
                return;

            ErrorMessage = "";

            try
            {
                LoadingData = true;
                InventoryDistribution = await InventoryService.GetInventoryDistribution(_inventoryId.Value);
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

        public void Hide()
        {
            if (Modal == null)
                return;

            Modal.HideAsync();
        }
    }
}
