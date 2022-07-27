using FluentValidation;

namespace WarehouseMgmt.Client.ViewModels
{
    public class WarehouseViewModel
    {
        public string Name { get; set; } = "";

        public string StreetAddress { get; set; } = "";

        public string City { get; set; } = "";

        public string State { get; set; } = "";

        public string ZipCode { get; set; } = "";

        public string Country { get; set; } = "";
    }

    public class WarehouseValidator : AbstractValidator<WarehouseViewModel>
    {
        public WarehouseValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Please enter a name.");

            RuleFor(x => x.StreetAddress)
                .NotEmpty()
                .WithMessage("Please enter a street address.");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Please enter a city.");

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("Please enter a state.");

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("Please enter a zip code.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Please enter a country.");
        }
    }
}
