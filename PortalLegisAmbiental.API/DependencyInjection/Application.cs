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
        }

        internal static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AddTipoAtoRequest>, AddTipoAtoValidator>();
            services.AddScoped<IValidator<TipoAtoRequest>, UpdateTipoAtoValidator>();
        }
    }
}
