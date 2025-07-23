using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsProjectAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace StudentsProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _studentDbContext;
        public StudentsController(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }

        [HttpPost]
        public IActionResult CreateStudent(CreateStudentDTO request)
        {
            Department department = _studentDbContext.Departments.FirstOrDefault(d => d.Id == request.DepartmentId);
            if (department != null)
            {
                Student student = new Student();
                student.Name = request.Name;
                student.DepartmentId = request.DepartmentId;

                _studentDbContext.Students.Add(student);
                _studentDbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound("Department does not exist");
            }

        }

        [HttpGet]
        public IActionResult GetStudentById(int id) 
        {
            Student student = _studentDbContext.Students.Include(s => s.Department).FirstOrDefault(std => std.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            else
            {
                GetStudentByIdDTO returnedStudent = new GetStudentByIdDTO();
                returnedStudent.Id = student.Id;
                returnedStudent.Name = student.Name;
                returnedStudent.DepartmentId = student.DepartmentId;
                returnedStudent.DepartmentName = student.Department.Name;
                return Ok(returnedStudent);
            }
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentDbContext.Students
                .Include(s => s.Department)
                .Select(s => new GetStudentByIdDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    DepartmentId = s.DepartmentId,
                    DepartmentName = s.Department.Name
                })
                .ToList();

            return Ok(students);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO request, [FromServices] UserManager<IdentityUser> userManager, [FromServices] IConfiguration configuration)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // إنشاء التوكن
            var jwtSettings = configuration.GetSection("Jwt");
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
