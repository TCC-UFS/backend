using FluentValidation;
using PortalLegisAmbiental.Application.Services;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Application.Validators;
using PortalLegisAmbiental.Domain.Dtos.Requests;

namespace PortalLegisAmbiental.API.DependencyInjection
{
    internal static class Application
    {
        internal static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ITipoAtoService, TipoAtoService>();
            services.AddScoped<IPermissaoService, PermissaoService>();
            services.AddScoped<IJurisdicaoService, JurisdicaoService>();
            services.AddScoped<IGrupoService, GrupoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAtoService, AtoService>();
        }

        internal static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AddTipoAtoRequest>, AddTipoAtoValidator>();
            services.AddScoped<IValidator<UpdateTipoAtoRequest>, UpdateTipoAtoValidator>();

            services.AddScoped<IValidator<AddPermissaoRequest>, AddPermissaoValidator>();
            services.AddScoped<IValidator<UpdatePermissaoRequest>, UpdatePermissaoValidator>();

            services.AddScoped<IValidator<AddJurisdicaoRequest>, AddJurisdicaoValidator>();
            services.AddScoped<IValidator<UpdateJurisdicaoRequest>, UpdateJurisdicaoValidator>();

            services.AddScoped<IValidator<AddGrupoRequest>, AddGrupoValidator>();
            services.AddScoped<IValidator<UpdateGrupoRequest>, UpdateGrupoValidator>();

            services.AddScoped<IValidator<AddUsuarioRequest>, AddUsuarioValidator>();
            services.AddScoped<IValidator<UpdateUsuarioRequest>, UpdateUsuarioValidator>();

            services.AddScoped<IValidator<AddAtoRequest>, AddAtoValidator>();
            services.AddScoped<IValidator<UpdateAtoRequest>, UpdateAtoValidator>();
        }
    }
}
