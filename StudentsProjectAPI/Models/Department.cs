namespace StudentsProjectAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //navigational
        public ICollection<Student> Students { get; set; }
    }
}
