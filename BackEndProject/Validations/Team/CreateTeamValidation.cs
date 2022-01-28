using BackEndProject.ViewModels.Team;
using FluentValidation;

namespace BackEndProject.Validations.Team
{
    public class CreateTeamValidation:AbstractValidator<CreateTeamVM>
    {
        public CreateTeamValidation()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Photo).NotNull();
            RuleFor(x=>x.Position).NotEmpty().NotNull().MaximumLength(50);
        }
    }
}
