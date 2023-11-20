using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using PortalLegisAmbiental.Domain.Dtos.Responses;
using PortalLegisAmbiental.Domain.Exceptions;

namespace PortalLegisAmbiental.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionFilter(IHostEnvironment hostEnvironment) =>
            _hostEnvironment = hostEnvironment;

        public void OnException(ExceptionContext context)
        {
            var errorList = new List<dynamic>
            {
                new { Name = "KeyNotFoundException", Code = 404 },
                new { Name = "ValidationException", Code = 400 },
                new { Name = "UnauthorizedAccessException", Code = 401 },
                new { Name = "ArgumentException", Code = 400 },
                new { Name = "AccessViolationException", Code = 403 },
                new { Name = "ArgumentNullException", Code = 400 },
                new { Name = "InvalidDataException", Code = 422 },
                new { Name = "HttpRequestException", Code = 400 },
                new { Name = "PortalLegisDomainException", Code = 400 },
                new { Name = "UserUnauthorizedException", Code = 401 }
            };

            var exceptionType = context.Exception.GetType().Name;
            var err = errorList.Find(x => x.Name == exceptionType);

            var stCode = err != null ? err.Code : 500;
            var result = new ErrorResponse (
                context.HttpContext.Request.Path,
                new List<string>(),
                context.Exception.ToString());

            var message = context.Exception.Message;

            if (exceptionType == "ValidationException")
            {
                var splitedMessage = message.Split("--");
                for (var i = 1; i < splitedMessage.Length; i++)
                {
                    result.Errors.Add(splitedMessage[i].Split("Severity")[0].Trim());
                }
            }
            else
            {
                result.Errors.Add(message);
                result.InnerException = context.Exception.InnerException?.Message;
            }

            if (exceptionType == "PortalLegisDomainException")
            {
                var portalLegisEx = (PortalLegisDomainException)context.Exception;
                if (portalLegisEx?.StatusCode != null)
                {
                    stCode = portalLegisEx.StatusCode;
                }

                if (portalLegisEx?.Code != null)
                {
                    result.SetCode(portalLegisEx.Code);
                }
            }

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

            if (_hostEnvironment.IsProduction())
            {
                result.NoException();
            }

            var bodyJsonStr = JsonSerializer.Serialize(result, serializeOptions);
            context.Result = new ContentResult
            {
                Content = bodyJsonStr,
                ContentType = "application/json",
                StatusCode = (int)stCode
            };
        }
    }
}
