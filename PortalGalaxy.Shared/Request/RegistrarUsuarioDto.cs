using System.ComponentModel.DataAnnotations;

namespace PortalGalaxy.Shared.Request;

public class RegistrarUsuarioDto
{
    [Required(ErrorMessage = Constantes.CampoRequerido)]
    public string Usuario { get; set; } = default!;

    [Required(ErrorMessage = Constantes.CampoRequerido)]
    [Display(Name = "Nombre Completo")]
    public string NombresCompleto { get; set; } = default!;

    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = Constantes.CampoRequerido)]
    public string Telefono { get; set; } = default!;

    [Required(ErrorMessage = Constantes.CampoRequerido)]
    [Display(Name = "Numero de Documento")]
    public string NroDocumento { get; set; } = default!;

    [Required(ErrorMessage = Constantes.CampoRequerido)]
    public string Password { get; set; } = default!;

    [Compare(nameof(Password), ErrorMessage = "Las claves no coinciden")]
    public string ConfirmPassword { get; set; } = default!;

    public string CodigoDepartamento { get; set; } = default!;
    public string CodigoProvincia { get; set; } = default!;
    public string CodigoDistrito { get; set; } = default!;
}