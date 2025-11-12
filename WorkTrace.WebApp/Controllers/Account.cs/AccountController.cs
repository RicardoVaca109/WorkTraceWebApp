using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.ApiServices;
using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AccountController(AuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginResponse = await _authApiService.LoginAsync(request);

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                ViewBag.Error = "Credenciales inválidas";
                return View();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.Token);

            var isActiveClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "IsActive")?.Value;
            if (!bool.TryParse(isActiveClaim, out var isActive) || !isActive)
            {
                ViewBag.Error = "Usuario inactivo.";
                return View();
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Enum.TryParse<UserRoles>(roleClaim, out var userRole))
            {
                ViewBag.Error = "Rol inválido.";
                return View();
            }

            var allowedRoles = new[] { UserRoles.Administrador };
            if (!allowedRoles.Contains(userRole))
            {
                ViewBag.Error = "Su rol no tiene acceso a la aplicación web.";
                return View();
            }

            // Guardar en sesión
            HttpContext.Session.SetString("JWToken", loginResponse.Token);
            HttpContext.Session.SetString("UserRole", userRole.ToString());
            HttpContext.Session.SetString("UserName",
                jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
