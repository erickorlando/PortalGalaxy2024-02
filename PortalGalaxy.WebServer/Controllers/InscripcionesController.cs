using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Shared;
using PortalGalaxy.Shared.Request;

namespace PortalGalaxy.WebServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscripcionesController : ControllerBase
{
    private readonly IInscripcionService _service;
    private readonly ILogger<InscripcionesController> _logger;

    public InscripcionesController(IInscripcionService service, ILogger<InscripcionesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] BusquedaInscripcionRequest request)
    {
        var response = await _service.ListAsync(request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await _service.FindByIdAsync(id);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] InscripcionDtoRequest request)
    {
        var usuario = User.Claims.First(p => p.Type == ClaimTypes.Email).Value;
        
        _logger.LogInformation(usuario);
        
        var response = await _service.AddAsync(usuario, request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("masiva")]
    [Authorize(Roles = Constantes.RolAdministrador)]
    public async Task<IActionResult> PostMasiva([FromBody] InscripcionMasivaDtoRequest request)
    {
        var response = await _service.AddMasivaAsync(request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Put(int id, [FromBody] InscripcionDtoRequest request)
    {
        var response = await _service.UpdateAsync(User.Identity!.Name!, id, request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);

        return response.Success ? Ok(response) : BadRequest(response);
    }

}