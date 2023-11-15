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
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Length(2)
                .WithMessage("O tamanho deve ser de exatamente 2 caractéres.");

            RuleFor(jurisdicao => jurisdicao.Ambito)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(Utils.VerifyEnum<EAmbitoType>);

            RuleFor(jurisdicao => jurisdicao.Estado)
                .Cascade(CascadeMode.Stop)
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
                .Cascade(CascadeMode.Stop)
                .Length(2);

            RuleFor(jurisdicao => jurisdicao.Ambito)
                .Cascade(CascadeMode.Stop)
                .Must(Utils.VerifyEnum<EAmbitoType>);

            RuleFor(jurisdicao => jurisdicao.Estado)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(4)
                .MaximumLength(50)
                .When(jurisdicao => IsEstadual(jurisdicao.Ambito));

            RuleFor(jurisdicao => jurisdicao.Id)
                .Cascade(CascadeMode.Stop)
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
