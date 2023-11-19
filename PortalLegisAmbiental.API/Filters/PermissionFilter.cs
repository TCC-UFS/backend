using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PortalLegisAmbiental.Application.Services.Interfaces;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Enums;
using System.Text.Json;

namespace PortalLegisAmbiental.API.Filters
{
    public class PermissionFilter : IActionFilter
    {
        private readonly IAccessService _accessService;
        private readonly Dictionary<EScopeType, string[]> _scopes = new()
        {
            { EScopeType.Read, new string[] { "get" } },
            { EScopeType.Write, new string[] { "post", "put", "patch" } },
            { EScopeType.Delete, new string[] { "delete" } }
        };

        public PermissionFilter(IAccessService accessService)
        {
            _accessService = accessService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = _accessService.GetLoggedUser(context.HttpContext.User);

            if (user == null)
                ReturnError(context);
            else
            {
                bool allowed = false;
                foreach (var grupo in user.Grupos)
                {
                    foreach(var perm in grupo.Permissoes)
                    {
                        var method = context.HttpContext.Request.Method.ToLower();
                        var path = context.HttpContext.Request.Path.Value?.ToLower() ?? string.Empty;
                        var resource = perm.Recurso.ToLower();

                        if (IsPathAllowed(resource, path) && IsMethodAllowed(perm.Scope, method))
                        {
                            allowed = true;
                            break;
                        }
                    }

                    if (allowed)
                        break;
                }

                if (!allowed)
                    ReturnError(context);
            }
                
        }

        private bool IsMethodAllowed(EScopeType scope, string method)
        {
            if (scope == EScopeType.All)
                return true;

            if (_scopes[scope].Contains(method))
                return true;
            else
                return false;
        }

        private bool IsPathAllowed(string recurso, string path)
        {
            if (recurso == "all")
                return true;

            var pathArray = path.Split('/');
            if (pathArray.Contains(recurso))
                return true;
            else
                return false;
        }

        private void ReturnError(ActionExecutingContext context)
        {
            var result = new ErrorResponse(
                context.HttpContext.Request.Path,
                new List<string>() { "Acesso Negado." }, null);
            result.Code = "NOT_ALLOWED";

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

            var bodyJsonStr = JsonSerializer.Serialize(result, serializeOptions);
            context.Result = new ContentResult
            {
                Content = bodyJsonStr,
                ContentType = "application/json",
                StatusCode = 403
            };
        }
    }
}
