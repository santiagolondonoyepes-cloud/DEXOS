using DEXOS.API.Requests;
using FluentValidation;

namespace DEXOS.API.Validation;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.BranchId).NotEmpty();
        RuleFor(x => x.Number).NotEmpty().MaximumLength(50);
        RuleForEach(x => x.Items).SetValidator(new CreateOrderItemRequestValidator());
    }
}
