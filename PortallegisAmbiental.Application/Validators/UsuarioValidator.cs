using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddUsuarioValidator : AbstractValidator<AddUsuarioRequest>
    {
        public AddUsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(50);

            RuleFor(usuario => usuario.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(Utils.IsEmail);

            RuleFor(usuario => usuario.Senha)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(8);
        }
    }

    public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioRequest>
    {
        public UpdateUsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(4)
                .MaximumLength(50);

            RuleFor(usuario => usuario.Email)
                .Cascade(CascadeMode.Stop)
                .Must(Utils.IsEmail);

            RuleFor(usuario => usuario.Senha)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(8);

            RuleFor(usuario => usuario.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}
