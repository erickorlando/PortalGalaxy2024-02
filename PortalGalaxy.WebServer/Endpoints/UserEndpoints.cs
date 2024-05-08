using Microsoft.AspNetCore.WebUtilities;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Shared.Request;

namespace PortalGalaxy.WebServer.Endpoints;

public static class UserEndpoints
{
    /// <summary>
    /// Endpoints para la seguridad de la aplicacion
    /// </summary>
    /// <param name="routes"></param>
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/Users");

        group.MapPost("/login", async (IUserService service, LoginDtoRequest request) =>
        {
            var response = await service.LoginAsync(request);
            return Results.Ok(response);
        });
        
        group.MapPost("/register", async (IUserService service, RegistrarUsuarioDto request) =>
        {
            var response = await service.RegisterAsync(request);
            return Results.Ok(response);
        });

        group.MapPost("/sendTokenToResetPassword",
            async (IUserService service, GenerateTokenToResetDtoRequest request) =>
            {
                var response = await service.SendTokenToResetPasswordAsync(request);
                return Results.Ok(response);
            });
        
        group.MapPost("/resetPassword", async (IUserService service, ResetPasswordDtoRequest request) =>
        {
            var response = await service.ResetPasswordAsync(request);
            return Results.Ok(response);
        });
    }
}