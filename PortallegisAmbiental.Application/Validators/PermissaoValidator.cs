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
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(permissao => permissao.Scope)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(Utils.VerifyEnum<EScopeType>);
        }
    }

    public class UpdatePermissaoValidator : AbstractValidator<UpdatePermissaoRequest>
    {
        public UpdatePermissaoValidator()
        {
            RuleFor(permissao => permissao.Recurso)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(3)
                .MaximumLength(40);

            RuleFor(permissao => permissao.Scope)
                .Cascade(CascadeMode.Stop)
                .Must(Utils.VerifyEnum<EScopeType>);

            RuleFor(permissao => permissao.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}
