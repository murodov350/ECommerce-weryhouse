using EcommerceWarehouse.Server.DTOs;
using FluentValidation;

namespace EcommerceWarehouse.Server.Validations
{
    public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
    {
        public CreateSupplierRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Phone).MaximumLength(30);
            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email)).MaximumLength(200);
        }
    }
}
