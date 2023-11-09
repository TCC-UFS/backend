using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using PortalLegisAmbiental.API.DependencyInjection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Whitelabel.API", Version = "v1" });
});

// Mapping validation errors from MVC.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var response = new
        {
            Code = "REQUEST_ERROR",
            Instance = context.HttpContext.Request.Path.Value,
            Errors = new List<string>()
        };

        foreach (var (key, value) in context.ModelState)
        {
            if (value.Errors.Any())
            {
                var errMessage = "Campo inválido.";
                var err = value.Errors[0];
                if (err.ErrorMessage != null && err.ErrorMessage.Contains("required"))
                {
                    errMessage = "Campo obrigatório.";
                }

                // Quando a key é "$" significa que o erro está na má formação do JSON Body.
                if (key == "$")
                {
                    response.Errors.Add("Requisição inválida, verifique se o JSON do body está correto.");
                }
                else if (key != "request" || context.ModelState.Count == 1)
                {
                    response.Errors.Add($"{key.Replace("$.", string.Empty)}: {errMessage}");
                }

            }
        }

        return new BadRequestObjectResult(response);
    };
});

// Versioning
builder.Services.AddApiVersioning(config =>
{
    config.ReportApiVersions = true;
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(p =>
{
    p.GroupNameFormat = "'v'VVV";
    p.SubstituteApiVersionInUrl = true;
});

// Aplication Injections
builder.Services.AddRepositories();

// Cors Configuration
builder.Services.AddCors(options => options.AddPolicy("CorsAPI",
        corsBuilder => corsBuilder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        for (int i = 0; i < provider.ApiVersionDescriptions.Count; i++)
        {
            ApiVersionDescription? description = provider.ApiVersionDescriptions[i];
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("CorsAPI");

app.Run();
