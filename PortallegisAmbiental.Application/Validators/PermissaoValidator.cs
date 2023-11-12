using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddPermissaoValidator : AbstractValidator<AddPermissaoRequest>
    {
        public AddPermissaoValidator()
        {
            RuleFor(permissao => permissao.Recurso)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(permissao => permissao.Scope)
                .NotEmpty()
                .Must(Utils.VerifyEnum<EScopeType>);
        }
    }

    public class UpdatePermissaoValidator : AbstractValidator<UpdatePermissaoRequest>
    {
        public UpdatePermissaoValidator()
        {
            RuleFor(permissao => permissao.Recurso)
                .MinimumLength(3)
                .MaximumLength(40);

            RuleFor(permissao => permissao.Scope)
                .Must(Utils.VerifyEnum<EScopeType>);

            RuleFor(permissao => permissao.Id)
                .NotEmpty();
        }
    }
}
