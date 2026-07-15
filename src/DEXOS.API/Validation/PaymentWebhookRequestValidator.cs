using DEXOS.API.Requests;
using FluentValidation;

namespace DEXOS.API.Validation;

public sealed class PaymentWebhookRequestValidator : AbstractValidator<PaymentWebhookRequest>
{
    public PaymentWebhookRequestValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Provider).NotEmpty().MaximumLength(40);
        RuleFor(x => x.EventType).NotEmpty().MaximumLength(80);
        RuleFor(x => x.PaymentReference).NotEmpty().MaximumLength(120);
    }
}
