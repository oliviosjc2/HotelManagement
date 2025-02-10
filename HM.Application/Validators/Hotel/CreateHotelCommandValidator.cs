using FluentValidation;
using HM.Application.Commands.Hotel;

namespace HM.Application.Validators.Hotel
{
    public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
    {
        public CreateHotelCommandValidator()
        {
            RuleFor(rule => rule.Name)
                .NotEmpty().NotNull().WithMessage("O nome do hotel não pode ser vazio ou null.")
                .Length(5, 64).WithMessage("O nome do hotel precisa ter de 5 a 64 caracteres.");

            RuleFor(rule => rule.Email)
                .NotEmpty().NotNull().WithMessage("O email do hotel não pode ser vazio ou null.")
                .MaximumLength(255).WithMessage("O email do hotel precisa conter no máximo 255 caracteres")
                .EmailAddress().WithMessage("Email em formato incorreto.");
        }
    }
}
