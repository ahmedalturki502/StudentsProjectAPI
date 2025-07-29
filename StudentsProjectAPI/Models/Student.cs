using System.ComponentModel.DataAnnotations;

namespace StudentsProjectAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(100, ErrorMessage = "اسم الطالب يجب أن يكون أقل من 100 حرف")]
        [Display(Name = "اسم الطالب")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "القسم مطلوب")]
        [Display(Name = "القسم")]
        public int DepartmentId { get; set; }
        
        public Department? Department { get; set; }
    }
}
