namespace StudentsProjectAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        //navigational
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
