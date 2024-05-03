using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Services.Profiles;
using PortalGalaxy.Shared.Configuracion;
using PortalGalaxy.WebServer.Endpoints;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

const string corConfiguration = "Blazor";

builder.Services.Configure<AppSettings>(builder.Configuration);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(policy =>
{
    policy.AddPolicy(corConfiguration, config =>
    {
        config.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<PortalGalaxyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PortalGalaxy"));

    // Ignoramos los warning por los query filter configurados
    options.ConfigureWarnings(warnings =>
    {
        warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDb")));

// Configuramos ASP.NET Identity Core
builder.Services.AddIdentity<GalaxyIdentityUser, IdentityRole>(policies =>
    {
        policies.Password.RequireDigit = false;
        policies.Password.RequireLowercase = true;
        policies.Password.RequireUppercase = true;
        policies.Password.RequireNonAlphanumeric = false;
        policies.Password.RequiredLength = 8;

        policies.User.RequireUniqueEmail = true;

        policies.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        policies.Lockout.MaxFailedAccessAttempts = 3;
        policies.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


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

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ??
                                           throw new InvalidOperationException("No se configuro el SecretKey"));

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseCors(corConfiguration);

app.MapCategoriaEndpoints();
app.MapUserEndpoints();

using (var scope = app.Services.CreateScope())
{
    await UserDataSeeder.Seed(scope.ServiceProvider);
}


app.Run();