using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddTipoAtoValidator : AbstractValidator<AddTipoAtoRequest>
    {
        public AddTipoAtoValidator()
        {
            RuleFor(tipoAto => tipoAto.Nome)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(32);
        }
    }

    public class UpdateTipoAtoValidator : AbstractValidator<UpdateTipoAtoRequest>
    {
        public UpdateTipoAtoValidator()
        {
            RuleFor(tipoAto => tipoAto.Nome)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(8)
                .MaximumLength(32);

            RuleFor(tipoAto => tipoAto.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}
