using CollegeApp.Models;

namespace CollegeApp.Services
{
    public interface IUserService
    {
        (string passwordHash, string salt) CreatePasswordHashWithSalt(string password);
        Task<bool> CreateUserAsync(UserDTO dto);
        Task<List<UserReadonlyDTO>> GetUserAsync();
        Task<UserReadonlyDTO> GetUserByIdAsync(int id);
        Task<UserReadonlyDTO> GetUserByUsernameAsync(string username);
    }
}
