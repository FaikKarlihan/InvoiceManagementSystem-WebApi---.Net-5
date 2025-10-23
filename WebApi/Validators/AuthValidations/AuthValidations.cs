using FluentValidation;
using WebApi.Dtos;

namespace WebApi.Validators
{
    public class LoginRequestValdator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValdator()
        {
            RuleFor(x => x.Mail)
                .NotNull().WithMessage("Mail cannot be null.")
                .NotEmpty().WithMessage("Mail cannot be empty.");
            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password cannot be null.")
                .NotEmpty().WithMessage("Password cannot be empty.");
        }
    } 
}