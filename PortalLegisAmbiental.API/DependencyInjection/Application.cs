using FluentValidation;
using PortalLegisAmbiental.Application.Services;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Application.Validators;
using PortalLegisAmbiental.Domain.Dtos.Requests;
using System.Security.Cryptography.X509Certificates;

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
            services.AddScoped<IAccessService, AccessService>();
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

        internal static void AddElasticClient(this IServiceCollection services)
        {

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            var path = "/var/www/ssl/projetosufs.cloud/es2/cert.pem";

            if (File.Exists(path))
            {
                handler.ClientCertificates.Add(new X509Certificate2(path));
                handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            }

            services.AddHttpClient("Elastic", client =>
            {
                var uri = Environment.GetEnvironmentVariable("ELASTIC_URL");
                var authString = Environment.GetEnvironmentVariable("ELASTIC_TOKEN");

                if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(authString))
                    throw new NotSupportedException("Um erro interno ocorreu.");

                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            }).ConfigurePrimaryHttpMessageHandler(x => handler); ;
        }
    }
}
