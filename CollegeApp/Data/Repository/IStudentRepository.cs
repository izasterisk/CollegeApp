﻿namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
        //Task<List<Student>> GetAllAsync();
        //Task<Student> GetByIdAsync(int id, bool useNoTracking = false);
        //Task<Student> GetByNameAsync(String name);
        //Task<int> CreateAsync(Student student);
        //Task<int> UpdateAsync(Student student);
        //Task<bool> DeleteAsync(Student student);

        Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus);
    }
}
