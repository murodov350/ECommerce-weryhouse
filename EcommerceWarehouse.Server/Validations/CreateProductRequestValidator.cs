using EcommerceWarehouse.Server.DTOs;
using FluentValidation;

namespace EcommerceWarehouse.Server.Validations
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Sku).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
