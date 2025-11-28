using System;
using FluentValidation;
using WebApi.Dtos;

namespace WebApi.Validators
{
    public class InvoiceCreateDtoValidator : AbstractValidator<InvoiceCreateDto>
    {
        public InvoiceCreateDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotNull().WithMessage("Type cannot be null.")
                .NotEmpty().WithMessage("Type cannot be empty.");

            RuleFor(x => x.PaymentInfo)
                .NotNull().WithMessage("PaymentInfo cannot be null.")
                .NotEmpty().WithMessage("PaymentInfo cannot be empty.");

            RuleFor(x => x.ApartmentNumber)
                .NotNull().WithMessage("ApartmentNumber cannot be null.")
                .NotEmpty().WithMessage("ApartmentNumber cannot be empty.");

            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount cannot be null.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Month)
                .NotNull().WithMessage("Month cannot be null.")
                .GreaterThanOrEqualTo(DateTime.Now.Month).WithMessage("Month must be current or future.")
                .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .NotNull().WithMessage("Year cannot be null.")
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Year must be current or future.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("DueDate cannot be empty.")
                .GreaterThan(DateTime.Now).WithMessage("DueDate must be a future date.");
        }
    } 
    public class DuesCreateDtoValidator : AbstractValidator<DuesCreateDto>
    {
        public DuesCreateDtoValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull().WithMessage("Amount cannot be null.")
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Month)
                .NotNull().WithMessage("Month cannot be null.")
                .GreaterThanOrEqualTo(DateTime.Now.Month).WithMessage("Month must be current or future.")
                .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .NotNull().WithMessage("Year cannot be null.")
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Year must be current or future.");   
        }
    }
}