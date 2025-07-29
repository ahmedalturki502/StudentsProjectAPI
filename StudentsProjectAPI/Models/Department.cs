namespace StudentsProjectAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigational property
        public ICollection<Student> Students { get; set; }   // يعني مجموعة من الطلاب المرتبطين لكل قسم واحد
    }
}
