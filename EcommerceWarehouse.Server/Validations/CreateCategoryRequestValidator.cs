using EcommerceWarehouse.Server.DTOs;
using FluentValidation;

namespace EcommerceWarehouse.Server.Validations
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
