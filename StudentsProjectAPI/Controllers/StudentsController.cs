using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsProjectAPI.Models.DTO;


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
            Student student = new Student();
            student.Name = request.Name;
            student.DepartmentId = request.DepartmentId;

            _studentDbContext.Students.Add(student);
            _studentDbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult GetStudentById(int id) 
        {
            Student student = _studentDbContext.Students.FirstOrDefault(std=>std.Id== id);
            return Ok(student);
        }


    }
}
