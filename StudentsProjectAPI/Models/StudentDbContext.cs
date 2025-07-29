using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentsProjectAPI.Models
{
    public class StudentDbContext : IdentityDbContext<IdentityUser>   /*contains DBcontext Propertice + ASB.NET identity like Roles + Users + Claims*/
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)     //Constructor : مرر إعدادات الاتصال وكل شيء مرتبط بالـ (دي بي كونتكست) إلى الكلاس الأساسي اللي مشتق منه، عشان يقدر يتصرف بشكل صحيح

        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Department>().HasData(    // Seeding
                 new Department()
                 {
                     Id = 1,
                     Name = "CS"
                 },
                 new Department()
                 {
                     Id = 2,
                     Name = "IT"
                 },
                 new Department()
                 {
                     Id = 3,
                     Name = "IS"
                 },
                 new Department()
                 {
                     Id = 4,
                     Name = "ML"
                 }
                );

        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
