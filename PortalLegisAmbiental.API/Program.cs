using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PortalLegisAmbiental.API.DependencyInjection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton(config);
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Whitelabel.API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor insira um token válido.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
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

// Authorization
var key = Encoding.ASCII.GetBytes(config.GetValue<string>("JWT:SecretKey"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddHttpClient("Elastic", client =>
{
    var uri = Environment.GetEnvironmentVariable("ELASTIC_URL");
    var authString = Environment.GetEnvironmentVariable("ELASTIC_TOKEN");

    if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(authString))
        throw new NotSupportedException("Um erro interno ocorreu.");

    client.BaseAddress = new Uri(uri);
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
});

// Aplication Injections
builder.Services.AddServices();
builder.Services.AddValidators();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("CorsAPI");

app.Run();
