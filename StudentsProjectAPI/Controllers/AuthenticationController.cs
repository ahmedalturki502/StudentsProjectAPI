using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentsProjectAPI.Models;
using StudentsProjectAPI.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentsProjectAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly StudentDbContext _studentDbContext; //تقدر تعطي وصول للجداول اللي انا سويت لها انشاء فقط
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager; //اقدر اوصل منها للجداول حقت مايكروسوفت (ASPNET)
        private readonly SignInManager<IdentityUser> _signInManager;
        public AuthenticationController(
            StudentDbContext studentDbContext,
            IConfiguration configuration,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _studentDbContext = studentDbContext;
            _configuration = configuration; //read from appsetting
            _userManager = userManager;    // responsible for user management like create delete or update
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request)
        {     /*يتحقق لو كان الايميل مسجل من قبل*/
            
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {      //400
                return BadRequest("User already exists");
            }
            /*إنشاء كائن مستخدم جديد*/
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            /*تضيف البيانات لقاعدة البيانات*/
            var result = await _userManager.CreateAsync(user, request.Password);
            /*لو فيه خطأ زي الباسوورد ما يحقق الشروط : يرجع الأخطاء للمستخدم*/
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password");
            }

            // إعدادات JWT
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenString });
        }


    }
}
