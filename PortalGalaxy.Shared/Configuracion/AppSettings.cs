#nullable disable
namespace PortalGalaxy.Shared.Configuracion;

public class AppSettings
{
    public SmtpConfiguration SmtpConfiguration { get; set; }
}

public class SmtpConfiguration
{
    public string Servidor { get; set; }
    public string Remitente { get; set; }
    public string Usuario { get; set; }
    public string Password { get; set; }
    public int Puerto { get; set; }
    public bool UsarSsl { get; set; }
}