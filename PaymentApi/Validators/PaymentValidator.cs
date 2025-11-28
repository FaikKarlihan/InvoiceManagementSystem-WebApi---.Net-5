using FluentValidation;
using PaymentApi.Models.DTOs;

namespace PaymentApi.Validators
{
    public class ConfirmPaymentDtoValidator : AbstractValidator<ConfirmPaymentDto>
    {
        public ConfirmPaymentDtoValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number cannot be blank!")
                .NotNull().WithMessage("Card number cannot be null!")
                .MaximumLength(16).WithMessage("The card number can be up to 16 digits!")
                .MinimumLength(15).WithMessage("Card number must be at least 15 digits.");
            RuleFor(x => x.CardPassword)
                .NotEmpty().WithMessage("Password cannot be blank!")
                .NotNull().WithMessage("Password cannot be null!")
                .MaximumLength(4).WithMessage("The Password can be up to 4 digits!")
                .MinimumLength(4).WithMessage("Password must be at least 4 digits.");
        }
    }
}