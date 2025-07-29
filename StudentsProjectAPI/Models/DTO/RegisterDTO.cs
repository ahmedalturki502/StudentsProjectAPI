using System.ComponentModel.DataAnnotations;

namespace StudentsProjectAPI.Models.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون {2} أحرف على الأقل", MinimumLength = 6)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }
    }
}
