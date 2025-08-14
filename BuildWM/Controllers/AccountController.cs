using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BuildWM.Data;
using BuildWM.Models;
using Microsoft.EntityFrameworkCore;
using BuildWM.DTOs;

namespace BuildWM.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var userExists = _context.Users.Any(u => u.Email == dto.Email);
            if (userExists)
            {
                ModelState.AddModelError("Email", "Email already registered.");
                return View(dto);
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid login attempt.");
                return View(dto);
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Password", "Invalid login attempt.");
                return View(dto);
            }

            // TODO: Set session or JWT or cookie

            return RedirectToAction("Index", "Home");
        }
    }
}
