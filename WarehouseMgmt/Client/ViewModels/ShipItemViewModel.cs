using FluentValidation;

namespace WarehouseMgmt.Client.ViewModels
{
    public class ShipItemViewModel
    {
        public int? IdWarehouse { get; set; } = null;
    }

    public class ShipItemValidator : AbstractValidator<ShipItemViewModel>
    {
        public ShipItemValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.IdWarehouse)
                .NotNull()
                .WithMessage("Please select a warehouse!")
                .GreaterThan(0)
                .WithMessage("Please select a warehouse!");
        }
    }
}
