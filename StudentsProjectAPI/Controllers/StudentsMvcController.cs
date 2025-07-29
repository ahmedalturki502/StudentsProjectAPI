using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StudentsProjectAPI.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsProjectAPI.Controllers
{
    [Authorize]
    public class StudentsMvcController : Controller
    {
        private readonly StudentDbContext _context;
        public StudentsMvcController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: /StudentsMvc
        public async Task<IActionResult> Index()
        {
            try
            {
                var students = await _context.Students.Include(s => s.Department).ToListAsync();
                return View(students);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"خطأ في تحميل الطلاب: {ex.Message}";
                return View(new List<Student>());
            }
        }

        // GET: /StudentsMvc/Test
        public async Task<IActionResult> Test()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                var students = await _context.Students.ToListAsync();
                
                var result = $"عدد الأقسام: {departments.Count}\n";
                result += $"عدد الطلاب: {students.Count}\n\n";
                
                result += "الأقسام:\n";
                foreach (var dept in departments)
                {
                    result += $"- {dept.Id}: {dept.Name}\n";
                }
                
                result += "\nالطلاب:\n";
                foreach (var student in students)
                {
                    result += $"- {student.Id}: {student.Name} (قسم: {student.DepartmentId})\n";
                }
                
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content($"خطأ: {ex.Message}");
            }
        }

        // GET: /StudentsMvc/AddTestStudent
        public async Task<IActionResult> AddTestStudent()
        {
            try
            {
                var testStudent = new Student
                {
                    Name = "طالب تجريبي",
                    DepartmentId = 1 // قسم CS
                };

                _context.Students.Add(testStudent);
                await _context.SaveChangesAsync();
                
                return Content($"تم إضافة طالب تجريبي بنجاح! ID: {testStudent.Id}");
            }
            catch (Exception ex)
            {
                return Content($"خطأ في إضافة طالب تجريبي: {ex.Message}");
            }
        }

        // GET: /StudentsMvc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.Include(s => s.Department).FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // GET: /StudentsMvc/Create
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        // POST: /StudentsMvc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                // التحقق من البيانات
                if (string.IsNullOrWhiteSpace(student.Name))
                {
                    ModelState.AddModelError("Name", "اسم الطالب مطلوب");
                }
                
                if (student.DepartmentId <= 0)
                {
                    ModelState.AddModelError("DepartmentId", "يجب اختيار قسم");
                }

                if (ModelState.IsValid)
                {
                    // إنشاء طالب جديد
                    var newStudent = new Student
                    {
                        Name = student.Name.Trim(),
                        DepartmentId = student.DepartmentId
                    };

                    _context.Students.Add(newStudent);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = $"تم إضافة الطالب '{newStudent.Name}' بنجاح!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"حدث خطأ أثناء إضافة الطالب: {ex.Message}");
            }
            
            // إعادة تحميل الأقسام في حالة الخطأ
            ViewBag.Departments = _context.Departments.ToList();
            return View(student);
        }

        // GET: /StudentsMvc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            ViewBag.Departments = _context.Departments.ToList();
            return View(student);
        }

        // POST: /StudentsMvc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _context.Departments.ToList();
            return View(student);
        }

        // GET: /StudentsMvc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.Include(s => s.Department).FirstOrDefaultAsync(m => m.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /StudentsMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 