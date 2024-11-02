using Domain.Models;
using FluentValidation;

namespace Infrastructure.Validators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        public ApplicationValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
