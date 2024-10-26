using Domain.Models;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class VersionInfoValidator : AbstractValidator<VersionInfo>
    {
        public VersionInfoValidator()
        {
            RuleFor(x => x.Version).NotEmpty().NotNull().Matches("^\\d+\\.\\d+\\.\\d+\\.\\d+$");
        }
    }
}