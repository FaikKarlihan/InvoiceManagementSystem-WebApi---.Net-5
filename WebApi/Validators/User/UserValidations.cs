using FluentValidation;
using WebApi.Dtos;

namespace WebApi.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        // CREATE USER
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name cannot be null.")
                .NotEmpty().WithMessage("Name cannot be empty.");

            RuleFor(x => x.Surname)
                .NotNull().WithMessage("Surname cannot be null.")
                .NotEmpty().WithMessage("Surname cannot be empty.");

            RuleFor(x => x.NationalId)
                .NotNull().WithMessage("NationalId cannot be null.")
                .NotEmpty().WithMessage("NationalId cannot be empty.");

            RuleFor(x => x.Mail)
                .NotNull().WithMessage("Mail cannot be null.")
                .NotEmpty().WithMessage("Mail cannot be empty.");

            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("Phone number cannot be null.")
                .NotEmpty().WithMessage("Phone number cannot be empty.");

            RuleFor(x => x.Role)
                .NotNull().WithMessage("Role cannot be null.")
                .NotEmpty().WithMessage("Role cannot be empty.");
        }
    }

    // SEND MESSAGE 
    public class UserMessageDtoValidator : AbstractValidator<UserMessageDto>
    {
        public UserMessageDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull().WithMessage("Title cannot be null.")
                .NotEmpty().WithMessage("Title cannot be empty.");
            RuleFor(x => x.Content)
                .NotNull().WithMessage("Content cannot be null.")
                .NotEmpty().WithMessage("Content cannot be empty.");
        }
    }
}