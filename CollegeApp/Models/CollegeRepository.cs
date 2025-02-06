namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>{
                new Student
                {
                    Id = 1,
                    studentName = "Test",
                    Email = "taivishao@gmail.com",
                    Address = "Noi dan choi bi bo roi"
                }
            };
    }
}
