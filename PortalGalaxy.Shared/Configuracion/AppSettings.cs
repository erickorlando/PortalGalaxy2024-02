#nullable disable
namespace PortalGalaxy.Shared.Configuracion;

public class AppSettings
{
    public string UrlFrontend { get; set; }
    public SmtpConfiguration SmtpConfiguration { get; set; }
    public Jwt Jwt { get; set; }
    public StorageConfiguration StorageConfiguration { get; set; }
}

public class Jwt
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
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

public class StorageConfiguration
{
    public string Path { get; set; }
    public string PublicUrl { get; set; }
}