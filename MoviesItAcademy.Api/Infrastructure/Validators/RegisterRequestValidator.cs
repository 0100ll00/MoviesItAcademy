using FluentValidation;
using MoviesItAcademy.Api.Models.RequestModels;
using System.Linq;
using MoviesItAcademy.Api.Infrastructure.Localisations;

namespace MoviesItAcademy.Api.Infrastructure.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestModel>
    {
        private const int MinimumPasswordLength = 8;
        private const int MinimumEmailLength = 10;
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ErrorMessages.NotEmpty + nameof(RegisterRequestModel.Email) + ".")
                .EmailAddress().WithMessage(ErrorMessages.EmailAddress)
                .MinimumLength(10).WithMessage(ErrorMessages.MinLength + MinimumEmailLength + ".");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ErrorMessages.NotEmpty + nameof(RegisterRequestModel.Password) + ".")
                .MinimumLength(MinimumPasswordLength).WithMessage(ErrorMessages.MinLength + MinimumPasswordLength + ".")
                .Must(p => p.Any(char.IsDigit)).WithMessage(ErrorMessages.MustDigit)
                .Must(p => p.Any(char.IsUpper)).WithMessage(ErrorMessages.MustUppercase);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(ErrorMessages.NotEmpty + nameof(RegisterRequestModel.ConfirmPassword) + ".")
                .Matches(y => y.Password).WithMessage(ErrorMessages.MustMatch + nameof(RegisterRequestModel.Password) + ".");
        }
    }
}
