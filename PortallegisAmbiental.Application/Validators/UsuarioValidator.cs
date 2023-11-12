using FluentValidation;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.Application.Validators
{
    public class AddUsuarioValidator : AbstractValidator<AddUsuarioRequest>
    {
        public AddUsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(50);

            RuleFor(usuario => usuario.Email)
                .NotEmpty()
                .Must(Utils.IsEmail);

            RuleFor(usuario => usuario.Senha)
                .NotEmpty()
                .MinimumLength(8);
        }
    }

    public class UpdateUsuarioValidator : AbstractValidator<UpdateUsuarioRequest>
    {
        public UpdateUsuarioValidator()
        {
            RuleFor(usuario => usuario.Nome)
                .MinimumLength(4)
                .MaximumLength(50);

            RuleFor(usuario => usuario.Email)
                .Must(Utils.IsEmail);

            RuleFor(usuario => usuario.Senha)
                .MinimumLength(8);

            RuleFor(usuario => usuario.Id)
                .NotEmpty();
        }
    }
}
