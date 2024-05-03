using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Shared;
using PortalGalaxy.Shared.Request;
using PortalGalaxy.Shared.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace PortalGalaxy.Services.Implementaciones;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<GalaxyIdentityUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly IAlumnoRepository _alumnoRepository;
    private readonly IEmailService _emailService;

    public UserService(IConfiguration configuration, UserManager<GalaxyIdentityUser> userManager, 
        ILogger<UserService> logger, IAlumnoRepository alumnoRepository, IEmailService emailService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _logger = logger;
        _alumnoRepository = alumnoRepository;
        _emailService = emailService;
    }
    public async Task<LoginDtoResponse> LoginAsync(LoginDtoRequest request)
    {
        var response = new LoginDtoResponse();

        try
        {
            var identity = await _userManager.FindByNameAsync(request.Usuario);

            if (identity is null)
                throw new SecurityException("Usuario no existe");

            if (await _userManager.IsLockedOutAsync(identity))
            {
                throw new SecurityException($"Demasiados intentos fallidos para el usuario {request.Usuario}");
            }

            // Validamos el usuario y clave.
            if (!await _userManager.CheckPasswordAsync(identity, request.Password))
            {
                response.ErrorMessage = "Usuario o clave incorrecta";
                _logger.LogWarning($"Intento fallido de Login para el usuario {identity.UserName}");
                
                await _userManager.AccessFailedAsync(identity); // Esto aumenta el contador de bloqueo

                return response;
            }

            var roles = await _userManager.GetRolesAsync(identity);
            var fechaExpiracion = DateTime.Now.AddHours(1);

            // Vamos a devolver los Claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, identity.NombreCompleto),
                new Claim(ClaimTypes.Email, identity.Email!),
                new Claim(ClaimTypes.Expiration, fechaExpiracion.ToString("yyyy-MM-dd HH:mm:ss")),
            };

            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            response.Roles = roles.ToList();

            // Creamos el JWT
            var llaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credenciales = new SigningCredentials(llaveSimetrica, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credenciales);

            var payload = new JwtPayload(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                DateTime.Now,
                fechaExpiracion
            );

            var jwtToken = new JwtSecurityToken(header, payload);

            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            response.NombreCompleto = identity.NombreCompleto;
            response.Success = true;

            _logger.LogInformation("Se creó el JWT de forma satisfactoria");
        }
        catch (SecurityException ex)
        {
            response.ErrorMessage = ex.Message;
            _logger.LogError(ex, "Error de seguridad {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error inesperado";
            _logger.LogError(ex, "Error al autenticar {Message}", ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse> RegisterAsync(RegistrarUsuarioDto request)
    {
        var response = new BaseResponse();

        try
        {
            var identity = new GalaxyIdentityUser
            {
                NombreCompleto = request.NombresCompleto,
                UserName = request.Usuario,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identity, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identity, Constantes.RolAlumno);

                var alumno = new Alumno
                {
                    NombreCompleto = request.NombresCompleto,
                    Correo = request.Email,
                    NroDocumento = request.NroDocumento,
                    Departamento = request.CodigoDepartamento,
                    Provincia = request.CodigoProvincia,
                    Distrito = request.CodigoDistrito,
                    FechaInscripcion = DateTime.Today
                };

                await _alumnoRepository.AddAsync(alumno);
                
                // Enviar un email
                await _emailService.SendEmailAsync(request.Email, "Portal Galaxy - Registro",
                    $@"<strong><p>Felicidades {request.NombresCompleto}</p></strong>
                     <p>Su cuenta ha sido creada satisfactoriamente</p>");
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var identityError in result.Errors)
                {
                    sb.AppendFormat("{0} ", identityError.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Clear();
            }

            response.Success = result.Succeeded;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al registrar";
            _logger.LogWarning(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<string>> SendTokenToResetPasswordAsync(GenerateTokenToResetDtoRequest request)
    {
        var response = new BaseResponseGeneric<string>();
        try
        {
            GalaxyIdentityUser? user = null;
            
            if (!string.IsNullOrEmpty(request.Usuario))
            {
                user = await _userManager.FindByNameAsync(request.Usuario);
                if (user is null) throw new SecurityException("Usuario no existe");
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null) throw new SecurityException("Usuario no existe");
            }

            if (user is null)
            {
                response.ErrorMessage = "Usuario no pudo ser validado";
                return response;
            }
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // TODO: Enviar correo electronico con el token de reseteo.
            
            await _emailService.SendEmailAsync(request.Email!, "Reset Password",
                $"Please use the following token to reset your password: {token}");
            
            response.Data = token;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al generar el token para restablecer la contraseña";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordDtoRequest request)
    {
        var response = new BaseResponse();
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new SecurityException("Usuario no existe");
            
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Clave);
            if (result.Succeeded)
            {
                response.Success = true;
                _logger.LogInformation("Contraseña restablecida con éxito");
                
                // TODO: Enviar un correo con el mensaje.
                await _emailService.SendEmailAsync(request.Email!, "Reset Password Confirmation",
                    "Your password has been successfully reset.");
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var identityError in result.Errors)
                {
                    sb.AppendFormat("{0} ", identityError.Description);
                }

                response.ErrorMessage = sb.ToString();
                sb.Clear();
                _logger.LogError("Error al restablecer la contraseña");
            }

            response.Success = result.Succeeded;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al restablecer la contraseña";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }
}