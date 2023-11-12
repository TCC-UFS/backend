using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddJurisdicaoValidator : AbstractValidator<AddJurisdicaoRequest>
    {
        public AddJurisdicaoValidator()
        {
            RuleFor(jurisdicao => jurisdicao.Sigla)
                .NotEmpty()
                .Length(2);

            RuleFor(jurisdicao => jurisdicao.Ambito)
                .NotEmpty()
                .Must(Utils.VerifyEnum<EAmbitoType>);

            RuleFor(jurisdicao => jurisdicao.Estado)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(50)
                .When(jurisdicao => IsEstadual(jurisdicao.Ambito));
        }

        private bool IsEstadual(string ambito)
        {
            if (!Enum.TryParse<EAmbitoType>(ambito, true, out var eAmbito))
                return false;

            return eAmbito.Equals(EAmbitoType.Estadual);
        }
    }

    public class UpdateJurisdicaoValidator : AbstractValidator<UpdateJurisdicaoRequest>
    {
        public UpdateJurisdicaoValidator()
        {
            RuleFor(jurisdicao => jurisdicao.Sigla)
                .Length(2);

            RuleFor(jurisdicao => jurisdicao.Ambito)
                .Must(Utils.VerifyEnum<EAmbitoType>);

            RuleFor(jurisdicao => jurisdicao.Estado)
                .MinimumLength(4)
                .MaximumLength(50)
                .When(jurisdicao => IsEstadual(jurisdicao.Ambito));

            RuleFor(jurisdicao => jurisdicao.Id)
                .NotEmpty();
        }

        private bool IsEstadual(string? ambito)
        {
            if (!Enum.TryParse<EAmbitoType>(ambito, true, out var eAmbito))
                return false;

            return eAmbito.Equals(EAmbitoType.Estadual);
        }
    }
}
