using BackEndProject.ViewModels.Team;
using FluentValidation;

namespace BackEndProject.Validations.Team
{
    public class UpdateTeamValidation:AbstractValidator<UpdateTeamVM>
    {
        public UpdateTeamValidation()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Position).NotEmpty().NotNull().MaximumLength(50);
        }
    }
}
