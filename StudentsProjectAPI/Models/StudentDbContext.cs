using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentsProjectAPI.Models
{
    public class StudentDbContext : IdentityDbContext<IdentityUser>   
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)       
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
