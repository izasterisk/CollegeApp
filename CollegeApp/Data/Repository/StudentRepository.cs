
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentRepository(CollegeDBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<int> CreateAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var studentDel = await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
            if (studentDel == null)
            {
                throw new ArgumentException($"No student found with id: {id}");
            }
            _dbContext.Students.Remove(studentDel);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dbContext.Students
                                   .Where(student => student.studentName.ToLower() == name.ToLower())
                                   .FirstOrDefaultAsync();
        }

        public async Task<int> updateAsync(Student student)
        {
            var studentUpdate = await _dbContext.Students.Where(s => s.Id == student.Id).FirstOrDefaultAsync();
            if (studentUpdate == null)
            {
                throw new ArgumentException($"No student found with id: {student.Id}");
            }
            studentUpdate.studentName = student.studentName;
            studentUpdate.Address = student.Address;
            studentUpdate.Email = student.Email;
            studentUpdate.DOB = student.DOB;
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
