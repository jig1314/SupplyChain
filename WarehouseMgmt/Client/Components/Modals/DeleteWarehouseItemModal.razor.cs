using BlazorStrap;
using Microsoft.AspNetCore.Components;

namespace WarehouseMgmt.Client.Components.Modals
{
    public partial class DeleteWarehouseItemModal : ComponentBase
    {
        private int _warehouseItemId;
        protected BSModal? Modal { get; set; }

        [Parameter]
        public EventCallback<int> OnDelete { get; set; }

        public void Show(int warehouseItemId)
        {
            if (Modal == null)
                return;

            _warehouseItemId = warehouseItemId;

            Modal.ShowAsync();
        }

        public void Hide()
        {
            if (Modal == null)
                return;

            Modal.HideAsync();
        }

        public async void Delete()
        {
            if (Modal == null)
                return;

            await Modal.HideAsync();

            await OnDelete.InvokeAsync(_warehouseItemId);
        }
    }
}
