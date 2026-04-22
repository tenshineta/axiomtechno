using axiomtechno.DTO;
using axiomtechno.Data;
using axiomtechno.Services;
using axiomtechno.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace axiomtechno.Controllers
{
    public class LoginController : Controller
    {
        private readonly axiomtechnoContext _context;

        public LoginController(axiomtechnoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Ingresar()
        {
            return View(new UsuarioDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Ingresar(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                return View(new UsuarioDTO());
            }

            var clave = PasswordService.HashPassword(usuarioDTO.UsPasswordHash?.ToString() ?? string.Empty);


            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(item => item.UsDni == usuarioDTO.UsDni && item.UsPasswordHash == clave);

            if (usuario != null)
            {
                var roles = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(item => item.UsDni == usuarioDTO.UsDni);

                var rolNombre = roles?.Rol?.RolNombre ?? "Usuario";
                usuarioDTO.Rol = rolNombre;

                var nombre = usuario.UsNombre ?? string.Empty;
                var apellido = usuario.UsApellido ?? string.Empty;

                var claims = new List<Claim>
                    {
                        new Claim("DNI", usuario.UsDni?.ToString() ?? string.Empty),
                        new Claim("Nombre", nombre),
                        new Claim("Apellido", apellido),
                        new Claim("ROL", usuarioDTO.Rol ?? string.Empty),
                        new Claim(ClaimTypes.Role, usuarioDTO.Rol ?? string.Empty),
                        new Claim(ClaimTypes.Name, $"{nombre} {apellido}".Trim())
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                usuarioDTO.UsAutenticado = true;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var userByDni = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsDni == usuarioDTO.UsDni);
                if (userByDni != null)
                {
                    TempData["LoginError"] = "Contraseña incorrecta. Intenta nuevamente.";
                }
                else
                {
                    TempData["LoginError"] = "Usuario no encontrado. Verifica tu DNI.";
                }

                return View(usuarioDTO);
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
