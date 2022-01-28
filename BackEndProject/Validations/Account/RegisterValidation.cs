using BackEndProject.ViewModels.Account;
using FluentValidation;

namespace BackEndProject.Validations.Account
{
    public class RegisterValidation:AbstractValidator<RegisterVM>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.FullName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x=>x.Email).EmailAddress().NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x=>x.Username).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty().Equal(x => x.ConfirmPassword).WithMessage("Password is not equal");
        }
    }
}
