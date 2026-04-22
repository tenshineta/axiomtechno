using axiomtechno.DTO;
using axiomtechno.Models;
using axiomtechno.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using axiomtechno.Data;
using axiomtechno.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace axiomtechno.Controllers
{
    public class AccessController : Controller
    {
        private readonly axiomtechnoContext _context;
        public AccessController(axiomtechnoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StartRecovery()
        {
            RecoveryViewModel model = new RecoveryViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StartRecovery(RecoveryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsCorreo == model.UsEmail);

            if (usuario == null)
            {
                TempData["MensajeError"] = "No se encontró una cuenta asociada a ese correo electrónico.";
                return View(model);
            }

            var token = Guid.NewGuid().ToString("N");

            var prevToken = usuario.token_recovery;
            usuario.token_recovery = token;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            try
            {
                await SendEmailAsync(usuario.UsCorreo, token);

                TempData["MensajeEnlace"] = "El enlace de recuperación se ha enviado a su correo registrado correctamente.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                usuario.token_recovery = prevToken;
                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["MensajeError"] = "No se pudo enviar el correo de recuperación. Compruebe la configuración de SMTP o contacte al administrador.";

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Recovery(string token, string result = null)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Token no válido.";
                return RedirectToAction("StartRecovery", "Access");
            }
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.token_recovery == token);

            if (usuario == null)
            {
                TempData["Error"] = "El enlace de recuperación es inválido o ha expirado.";
                return RedirectToAction("StartRecovery", "Access");
            }

            var model = new RecoveryPasswordViewModel
            {
                token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recovery(RecoveryPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dniStr = (model.UsDNI ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(dniStr))
            {
                ModelState.AddModelError(string.Empty, "Debe ingresar el DNI asociado a la cuenta.");
                return View(model);
            }

            if (!long.TryParse(dniStr, out var dniLong))
            {
                ModelState.AddModelError(string.Empty, "El DNI introducido no tiene un formato válido.");
                return View(model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.token_recovery == model.token);

            if (usuario == null)
            {
                TempData["Error"] = "Token inválido. Solicite un nuevo enlace de recuperación.";
                return RedirectToAction("StartRecovery", "Access");
            }

            var usuarioPorDni = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsDni == dniLong);

            if (usuarioPorDni == null)
            {
                ModelState.AddModelError(string.Empty, "No existe ningún usuario registrado con el DNI proporcionado.");
                return View(model);
            }

            if (!usuario.UsDni.HasValue || usuario.UsDni.Value != dniLong)
            {
                ModelState.AddModelError(string.Empty, "El DNI no coincide con la cuenta asociada al enlace de recuperación.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.UsPassword) ||
                string.IsNullOrWhiteSpace(model.UsPasswordConfirm) ||
                !model.UsPassword.Equals(model.UsPasswordConfirm, StringComparison.Ordinal))
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden o están vacías.");
                return View(model);
            }

            try
            {
                usuario.UsPasswordHash = PasswordService.HashPassword(model.UsPassword);
                usuario.token_recovery = null;

                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                TempData["RecoverySuccess"] = "Contraseña modificada con éxito. Ya puede iniciar sesión.";
                return View();
            }
            catch (DbUpdateException dbEx)
            {
                TempData["RecoveryError"] = $"Error al actualizar la contraseña: {dbEx.InnerException?.Message ?? dbEx.Message}";
                return RedirectToAction("Recovery", new { token = model.token, result = "error" });
            }
            catch (Exception ex)
            {
                TempData["RecoveryError"] = $"Error inesperado: {ex.Message}";
                return RedirectToAction("Recovery", new { token = model.token, result = "error" });
            }
        }

        private async Task SendEmailAsync(string EmailDestino, string token)
        {

            var url = Url.Action("Recovery", "Access", new { token = token }, Request.Scheme);
            var urlEscaped = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(url);

            var oMailMessage = new MailMessage(
                            "distribuidora606@gmail.com",
                            EmailDestino,
                            "Restablecimiento de contraseña – Sistema Distribuidora",
                            $@" 
                                        <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 650px; margin: 0 auto; padding: 20px;'> 
                                            <h2 style='color: #004d80;'>Solicitud de restablecimiento de contraseña</h2> 
                                            <p>Estimado/a usuario/a:</p> 
                                            <p>Recibimos una solicitud para restablecer la contraseña de su cuenta en el <strong>Sistema Distribuidora.</p> 
                                            <p>Si usted realizó esta solicitud, haga clic en el siguiente enlace para crear una nueva contraseña:</p> 
                                            <div style='text-align: center; margin: 25px 0;'> 
                                                <a href='{urlEscaped}'  
                                                   style='display: inline-block; padding: 12px 24px; background-color: #004d80; color: white; text-decoration: none; border-radius: 5px; font-weight: bold;'> 
                                                    Restablecer mi contraseña 
                                                </a> 
                                            </div> 
                                            <p>Este enlace es válido por una sola vez y expirará en 30 minutos.</p> 
                                            <p><strong>¿No solicitó este cambio?</strong> Si usted no ha solicitado restablecer su contraseña, por favor ignore este mensaje. Su cuenta permanecerá segura.</p> 
                                            <p>Para cualquier duda o asistencia adicional, no dude en contactar al staff de la Distribuidora.</p> 
                                            <hr style='border: 0; border-top: 1px solid #eee; margin: 30px 0;' /> 
                                            <p style='font-size: 0.9em; color: #666;'> 
                                                Distribuidora de snacks y bebidas<br> 
                                                <em>Llevando lo mejor a todos lados.</em> 
                                            </p> 
                                        </div>"
                            );

            oMailMessage.IsBodyHtml = true;

            using var oSmtpClient = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new System.Net.NetworkCredential("distribuidora606@gmail.com", "rfhhvkvubthwtvkp")
            };

            await oSmtpClient.SendMailAsync(oMailMessage);
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var usuarioDTO = new UsuarioDTO();

            return View(usuarioDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioDTO modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            if (await _context.Usuarios.AnyAsync(u => u.UsDni == modelo.UsDni))
            {
                TempData["MensajeError"] = "Ya existe este DNI en el sistema.";
                return View(modelo);
            }

            var hoy = DateTime.Today;
            var edad = hoy.Year - modelo.UsFechaNacimiento.Year;
            if (modelo.UsFechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            if (edad < 18)
            {
                TempData["MensajeError"] = "El usuario es menor de edad y no puede registrarse.";
                return View(modelo);
            }

            var rol = "Usuario";
            var rolEntity = await _context.Roles.FirstOrDefaultAsync(r => r.RolNombre == rol);

            if (rolEntity == null)
            {
                TempData["MensajeError"] = "El rol no existe en el sistema.";
                return View(modelo);
            }

            try
            {
                Usuarios usuario = new Usuarios()
                {
                    UsNombre = modelo.UsNombre,
                    UsApellido = modelo.UsApellido,
                    UsDni = modelo.UsDni,
                    UsCorreo = modelo.UsCorreo,
                    UsTelefono = modelo.UsTelefono,
                    UsPasswordHash = PasswordService.HashPassword(modelo.UsPasswordHash),
                    UsFechaNacimiento = modelo.UsFechaNacimiento,
                    RolID = rolEntity.RolId,
                    UsFechaCreacion = DateTime.Now,
                    UsActivo = true,
                };

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                TempData["MensajeExitoRegistro"] = "¡Registro exitoso! Ya puedes iniciar sesión.";
                return RedirectToAction(nameof(Registrarse));
            }
            catch (DbUpdateException ex)
            {
                TempData["MensajeError"] = $"Error al guardar el usuario: {ex.InnerException?.Message ?? ex.Message}";
                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error inesperado: {ex.Message}";
                return View(modelo);
            }
        }
    }
}
