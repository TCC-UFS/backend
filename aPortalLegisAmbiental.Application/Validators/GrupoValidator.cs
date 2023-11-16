using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddGrupoValidator : AbstractValidator<AddGrupoRequest>
    {
        public AddGrupoValidator()
        {
            RuleFor(grupo => grupo.Nome)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(20);
        }
    }

    public class UpdateGrupoValidator : AbstractValidator<UpdateGrupoRequest>
    {
        public UpdateGrupoValidator()
        {
            RuleFor(grupo => grupo.Nome)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(4)
                .MaximumLength(20);

            RuleFor(grupo => grupo.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}
