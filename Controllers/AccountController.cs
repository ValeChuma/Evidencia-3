using Microsoft.AspNetCore.Mvc;

namespace MiSitioWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "El email y la contraseña son requeridos.");
                return View();
            }

            // Validate email format
            if (!email.Contains("@"))
            {
                ModelState.AddModelError(string.Empty, "Ingresa un email válido.");
                return View();
            }

            // Validate password length
            if (password.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 6 caracteres.");
                return View();
            }

            // For development: accept any valid email and password
            HttpContext.Session.SetString("user_email", email);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "El email y la contraseña son requeridos.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View();
            }

            // TODO: Implement actual registration logic here
            TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicie sesión.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
