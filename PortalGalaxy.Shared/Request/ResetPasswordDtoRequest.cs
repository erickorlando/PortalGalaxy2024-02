using System.ComponentModel.DataAnnotations;

namespace PortalGalaxy.Shared.Request;

public class ResetPasswordDtoRequest
{
    [Required(ErrorMessage = Constantes.CampoRequerido)]
    public string Token { get; set; } = default!;

    [Required(ErrorMessage = Constantes.CampoRequerido)]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = Constantes.CampoRequerido)]
    public string Clave { get; set; } = default!;

    [Compare(nameof(Clave))]
    public string ConfirmarClave { get; set; } = default!;
}