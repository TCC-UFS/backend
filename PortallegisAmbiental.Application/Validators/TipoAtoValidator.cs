using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddTipoAtoValidator : AbstractValidator<AddTipoAtoRequest>
    {
        public AddTipoAtoValidator()
        {
            RuleFor(tipoAto => tipoAto.Nome)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(32);
        }
    }

    public class UpdateTipoAtoValidator : AbstractValidator<TipoAtoRequest>
    {
        public UpdateTipoAtoValidator()
        {
            RuleFor(tipoAto => tipoAto.Nome)
                .MinimumLength(8)
                .MaximumLength(32);

            RuleFor(tipoAto => tipoAto.Id)
                .NotEmpty();
        }
    }
}
