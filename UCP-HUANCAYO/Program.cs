using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UCP_HUANCAYO.Data;
using UCP_HUANCAYO.Dtos.Common;
using UCP_HUANCAYO.Helpers;
using UCP_HUANCAYO.Services;

var builder = WebApplication.CreateBuilder(args);

//servicios de negocio
builder.Services.AddScoped<AdministradoService>();
builder.Services.AddScoped<AlquilerService>();
builder.Services.AddScoped<ContratoService>();
builder.Services.AddScoped<CronogramaPagoService>();
builder.Services.AddScoped<PredioService>();
builder.Services.AddScoped<PredioImagenService>();
builder.Services.AddScoped<PredioTipoService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<DominioService>();
builder.Services.AddScoped<AuditoriaService>();

//Helpers
builder.Services.AddScoped<AuditoriaHelper>();
builder.Services.AddHttpContextAccessor();

//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// API y Swagger

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


// manejo de errores de validacion
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
