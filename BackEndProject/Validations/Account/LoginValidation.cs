using BackEndProject.ViewModels.Account;
using FluentValidation;

namespace BackEndProject.Validations.Account
{
    public class LoginValidation:AbstractValidator<LoginVm>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
