using FluentValidation;
using MoviesItAcademy.Api.Infrastructure.Localisations;
using MoviesItAcademy.Api.Models.RequestModels;
using System.Linq;

namespace MoviesItAcademy.Api.Infrastructure.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestModel>
    {
        private const int MinimumPasswordLength = 8;
        private const int MinimumUsernameLength = 4;
        public LoginRequestValidator()
        {
            
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(ErrorMessages.NotEmpty + nameof(LoginRequestModel.Username) + ".")
                .MinimumLength(4).WithMessage(ErrorMessages.MinLength + MinimumUsernameLength + ".");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ErrorMessages.NotEmpty + nameof(LoginRequestModel.Password) + ".")
                .MinimumLength(MinimumPasswordLength).WithMessage(ErrorMessages.MinLength + MinimumPasswordLength + ".")
                .Must(p => p.Any(char.IsDigit)).WithMessage(ErrorMessages.MustDigit)
                .Must(p => p.Any(char.IsUpper)).WithMessage(ErrorMessages.MustUppercase);
        }
    }
}
