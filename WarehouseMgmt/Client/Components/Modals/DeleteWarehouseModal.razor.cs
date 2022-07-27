using BlazorStrap;
using Microsoft.AspNetCore.Components;

namespace WarehouseMgmt.Client.Components.Modals
{
    public partial class DeleteWarehouseModal : ComponentBase
    {
        private int _warehouseId;
        protected BSModal? Modal { get; set; }

        [Parameter]
        public EventCallback<int> OnDelete { get; set; }

        public void Show(int warehouseId)
        {
            if (Modal == null)
                return;

            _warehouseId = warehouseId;

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

            await OnDelete.InvokeAsync(_warehouseId);
        }
    }
}
