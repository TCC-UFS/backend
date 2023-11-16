using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddAtoValidator : AbstractValidator<AddAtoRequest>
    {
        public AddAtoValidator()
        {
            RuleFor(ato => ato.Numero)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(15);

            RuleFor(ato => ato.Ementa)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(ato => ato.DataPublicacao)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(ato => ato.DataAto)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(ato => ato.JurisdicaoId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(ato => ato.TipoAtoId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }

    public class UpdateAtoValidator : AbstractValidator<UpdateAtoRequest>
    {
        public UpdateAtoValidator()
        {
            RuleFor(ato => ato.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(ato => ato.Numero)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(15);
        }
    }
}
