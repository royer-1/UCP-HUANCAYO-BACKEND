using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Common;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Services;
using UCP_HUANCAYO.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Servicios de negocio
builder.Services.AddScoped<AdministradoService>();
builder.Services.AddScoped<AlquilerService>();
builder.Services.AddScoped<ContratoService>();
builder.Services.AddScoped<CronogramaPagoService>();
builder.Services.AddScoped<PredioService>();
builder.Services.AddScoped<PredioImagenService>();
builder.Services.AddScoped<PredioTipoService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<DominioService>();
builder.Services.AddScoped<AuditoriaService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuditoriaHelper>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UsuarioContextHelper>();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticación con JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var config = builder.Configuration;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
    };
});

// politicas personalizadas
builder.Services.AddAuthorization(options =>
{
    // solo GET
    options.AddPolicy("SoloSupervisores", policy =>
        policy.RequireRole("supervisor", "superadministrador"));

    // GET, create, edit, desactivar
    options.AddPolicy("SoloGestores", policy =>
        policy.RequireRole("gestor", "administrador", "superadministrador"));

    // acceso total
    options.AddPolicy("SoloAdministradores", policy =>
        policy.RequireRole("administrador", "superadministrador"));

    // consultas GET
    options.AddPolicy("PuedeVerAdministrados", policy =>
        policy.RequireRole("supervisor", "gestor", "administrador", "superadministrador"));

    // acceso total a todo
    options.AddPolicy("SuperAdministradores", policy =>
        policy.RequireRole("superadministrador"));
});

builder.Services.AddAuthorization();

// Validación personalizada
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var response = new ApiErrorResponse("La validación falló", errors);
        return new BadRequestObjectResult(response);
    };
});

// Swagger con soporte JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "UCP-HUANCAYO", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT en el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("AllowAngular");

// Middleware en orden correcto
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UCP-HUANCAYO v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\":\"Acceso denegado. Tu rol no tiene permisos para esta acción.\"}");
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
