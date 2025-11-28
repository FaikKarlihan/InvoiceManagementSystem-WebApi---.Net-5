
using FluentValidation;
using WebApi.Dtos;

namespace WebApi.Validators
{
    public class HousingCreateDtoValidator : AbstractValidator<HousingCreateDto>
    {
        public HousingCreateDtoValidator()
        {
            RuleFor(x => x.ApartmentNumber)
                .NotNull().WithMessage("ApartmentNumber cannot be null.")
                .NotEmpty().WithMessage("ApartmentNumber cannot be empty.");
            RuleFor(x => x.Block)
                .NotNull().WithMessage("Block cannot be null.")
                .NotEmpty().WithMessage("Block cannot be empty.");
            RuleFor(x => x.Floor)
                .NotNull().WithMessage("Floor cannot be null.")
                .NotEmpty().WithMessage("Floor cannot be empty.");
            RuleFor(x => x.PlanType)
                .NotNull().WithMessage("PlanType cannot be null.")
                .NotEmpty().WithMessage("PlanType cannot be empty.");
        }
    }
}