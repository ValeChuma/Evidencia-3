using Microsoft.AspNetCore.Mvc;
using MiSitioWeb.Data;
using MiSitioWeb.Models;
using System.Security.Cryptography;
using System.Text;

namespace MiSitioWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _db;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "El usuario y la contraseña son requeridos.");
                return View();
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || !user.Activo)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View();
            }

            if (!VerifyPassword(password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View();
            }

            HttpContext.Session.SetString("user_username", user.Username);
            HttpContext.Session.SetString("user_email", user.Email);
            HttpContext.Session.SetString("user_id", user.Id.ToString());
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Todos los campos son requeridos.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View();
            }

            if (password.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "La contraseña debe tener al menos 6 caracteres.");
                return View();
            }

            if (!email.Contains("@"))
            {
                ModelState.AddModelError(string.Empty, "Ingresa un email válido.");
                return View();
            }

            if (_db.Users.Any(u => u.Username == username))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario con ese nombre.");
                return View();
            }

            if (_db.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un usuario con ese email.");
                return View();
            }

            var hash = HashPassword(password);
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hash,
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicie sesión.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Hash format: {iterations}.{saltBase64}.{hashBase64}
        private string HashPassword(string password, int iterations = 100_000)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);

            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = deriveBytes.GetBytes(32);

            return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        private bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;
            try
            {
                var parts = storedHash.Split('.');
                if (parts.Length != 3) return false;

                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] hash = Convert.FromBase64String(parts[2]);

                using var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
                byte[] testHash = deriveBytes.GetBytes(hash.Length);

                return CryptographicOperations.FixedTimeEquals(testHash, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}
