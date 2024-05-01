using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Services.Profiles;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PortalGalaxyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PortalGalaxy"));

    // Ignoramos los warning por los query filter configurados
    options.ConfigureWarnings(warnings =>
    {
        warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
    });
});

// Registramos las dependencias de Repositories y Services (SCRUTOR)
builder.Services.Scan(selector => selector
    .FromAssemblies(typeof(ICategoriaRepository).Assembly,
        typeof(ICategoriaService).Assembly)
    .AddClasses(false)
    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    .AsMatchingInterface()
    .WithTransientLifetime());

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CategoriaProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/categorias", async (ICategoriaService service) =>
{
    var response = await service.ListAsync();

    return Results.Ok(response);
});

app.MapDelete("api/categorias/{id:int}", async (ICategoriaRepository repository, int id) =>
{
    await repository.DeleteAsync(id);

    return Results.Ok();
});

app.MapGet("api/talleres", async (ITallerRepository repository) =>
{
    var collection = await repository.ListarAsync();

    return Results.Ok(collection);
});

app.Run();