﻿using PortalLegisAmbiental.Domain.Entities;
using PortalLegisAmbiental.Domain.Enums;

namespace PortalLegisAmbiental.Domain.IRepositories
{
    public interface IPermissaoRepository : IRepository
    {
        Task Add(Permissao permissao);
        Task<Permissao?> GetById(ulong id, bool noTracking = false);
        Task<List<Permissao>> SearchByResource(string recurso, bool noTracking = false);
        Task<List<Permissao>> SearchByScope(EScopeType scope, bool noTracking = false);
        Task<bool> Exists(Permissao permissao);
        Task Disable(Permissao permissao);
    }
}