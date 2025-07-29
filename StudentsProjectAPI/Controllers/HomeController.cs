using Microsoft.AspNetCore.Mvc;

namespace StudentsProjectAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // إذا كان المستخدم مسجل دخول، توجيه إلى صفحة الطلاب
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "StudentsMvc");
            }
            
            // إذا لم يكن مسجل دخول، توجيه إلى صفحة تسجيل الدخول
            return RedirectToAction("Login", "Account");
        }
    }
} 