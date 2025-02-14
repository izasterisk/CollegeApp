namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetByNameAsync(String name);
        Task<int> CreateAsync(Student student);
        Task<int> updateAsync(Student student);
        Task<bool> DeleteAsync(int id);
    }
}
