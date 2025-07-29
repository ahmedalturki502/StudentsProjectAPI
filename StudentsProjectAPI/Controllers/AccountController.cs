using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentsProjectAPI.Models.DTO;
using System.Linq;

namespace StudentsProjectAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "StudentsMvc");
                    else
                        ModelState.AddModelError("", "كلمة المرور غير صحيحة");
                }
                else
                {
                    ModelState.AddModelError("", "البريد الإلكتروني غير موجود");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني مسجل مسبقاً");
                    return View(model);
                }
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "StudentsMvc");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Test()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return Content($"مرحباً {User.Identity.Name}! أنت مسجل دخول بنجاح.");
            }
            else
            {
                return Content("أنت غير مسجل دخول.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateTestUser()
        {
            try
            {
                var testUser = new IdentityUser
                {
                    UserName = "test@test.com",
                    Email = "test@test.com"
                };

                var result = await _userManager.CreateAsync(testUser, "123456");
                if (result.Succeeded)
                {
                    return Content("تم إنشاء المستخدم التجريبي بنجاح. البريد: test@test.com، كلمة المرور: 123456");
                }
                else
                {
                    return Content($"فشل في إنشاء المستخدم: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            catch (Exception ex)
            {
                return Content($"خطأ: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
} 