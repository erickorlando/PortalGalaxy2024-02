using PortalGalaxy.Services.Interfaces;

namespace PortalGalaxy.WebServer.Endpoints;

public static class ReporteEndpoints
{
    public static void MapReportes(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/Reportes");

        group.MapGet("/TalleresPorMes/{anio:int}", async (ITallerService service, int anio) =>
        {
            var response = await service.ReporteTalleresPorMes(anio);
            return Results.Ok(response);
        });

        group.MapGet("/TalleresPorInstructor/{anio:int}", async (ITallerService service, int anio) =>
        {
            var response = await service.ReporteTalleresPorInstructor(anio);
            return Results.Ok(response);
        });
    }
    
}