﻿using PortalLegisAmbiental.Domain.IRepositories;
using PortalLegisAmbiental.Infrastructure.ElasticSearch;
using PortalLegisAmbiental.Infrastructure.MySQL;
using PortalLegisAmbiental.Infrastructure.MySQL.Repositories;

namespace PortalLegisAmbiental.API.DependencyInjection
{
    internal static class Infrastructure
    {
        internal static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<EfDbContext>();

            services.AddScoped<ITipoAtoRepository, TipoAtoRepository>();
            services.AddScoped<IPermissaoRepository, PermissaoRepository>();
            services.AddScoped<IJurisdicaoRepository, JurisdicaoRepository>();
            services.AddScoped<IGrupoRepository, GrupoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAtoRepository, AtoRepository>();
            services.AddTransient<ISearchRepository, SearchRepository>();
        }
    }
}
