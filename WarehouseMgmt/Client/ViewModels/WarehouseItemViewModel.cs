using FluentValidation;

namespace WarehouseMgmt.Client.ViewModels
{
    public class WarehouseItemViewModel
    {
        public string WarehouseName { get; set; } = "";

        public string StorageLocation { get; set; } = "";

        public string PartNumber { get; set; } = "";

        public string Description { get; set; } = "";

        public bool HasSerialNumber { get; set; } = false;

        public string SerialNumber { get; set; } = "";

        public string Qty { get; set; } = "";
    }

    public class WarehouseItemValidator : AbstractValidator<WarehouseItemViewModel>
    {
        public WarehouseItemValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.StorageLocation)
                .NotEmpty()
                .WithMessage("Please enter a storage location in the warehouse.");

            RuleFor(x => x.PartNumber)
                .NotEmpty()
                .WithMessage("Please enter a part number.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Please enter a description of the part.");

            When(x => x.HasSerialNumber,
                () =>
                {
                    RuleFor(x => x.SerialNumber)
                        .NotEmpty()
                        .WithMessage("Please enter a serial number.");
                })
                .Otherwise(() =>
                {
                    RuleFor(x => x.Qty)
                        .Custom((quantity, context) =>
                        {
                            if (string.IsNullOrWhiteSpace(quantity) || !int.TryParse(quantity, out int value) || value <= 0)
                            {
                                context.AddFailure($"{quantity} is not a valid quantity! Must be a whole number greater than 0!");
                            }
                        });
                });
        }
    }
}
