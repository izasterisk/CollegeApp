using CollegeApp.Data.Repository;
using CollegeApp.Data;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using CollegeApp.Models;

namespace CollegeApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<User> _userRepository;
        public UserService(ICollegeRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public (string passwordHash, string salt) CreatePasswordHashWithSalt(string password)
        {
            //Create salt
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            //Create hash
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            return (hash, Convert.ToBase64String(salt));
        }
        public async Task<bool> CreateUserAsync(UserDTO dto)
        {
            //Old way
            //if(dto == null)
            //{
            //    throw new ArgumentNullException(nameof(dto));
            //}
            //New way
            ArgumentNullException.ThrowIfNull(dto, $"{nameof(dto)} is null");
            var existingUser = await _userRepository.GetAsync(u => !u.IsDeleted && u.Username.Equals(dto.Username));
            if (existingUser != null)
            {
                throw new Exception($"Username {dto.Username} already exists.");
            }
            User user = _mapper.Map<User>(dto);
            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                user.Password = passwordHash.passwordHash;
                user.PasswordSalt = passwordHash.salt;
            }
            else
            {
                throw new ArgumentNullException(nameof(dto.Password), "Password cannot be null or empty.");
            }
            await _userRepository.CreateAsync(user);
            return true;
        }
        public async Task<List<UserReadonlyDTO>> GetUserAsync()
        {
            var users = await _userRepository.GetAllByFilterAsync(u => !u.IsDeleted);
            return _mapper.Map<List<UserReadonlyDTO>>(users);
        }
        public async Task<UserReadonlyDTO> GetUserByIdAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, $"{nameof(id)} is null");
            var user = await _userRepository.GetAsync(u => !u.IsDeleted && u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return _mapper.Map<UserReadonlyDTO>(user);
        }
        public async Task<UserReadonlyDTO> GetUserByUsernameAsync(string username)
        {
            ArgumentNullException.ThrowIfNull(username, $"{nameof(username)} is null");
            var user = await _userRepository.GetAsync(u => !u.IsDeleted && u.Username.Equals(username));
            if (user == null)
            {
                throw new KeyNotFoundException($"User with username {username} not found.");
            }
            return _mapper.Map<UserReadonlyDTO>(user);
        }
        public async Task<bool> UpdateUserAsync(UserDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto, $"{nameof(dto)} is null");
            var existingUser = await _userRepository.GetAsync(u => !u.IsDeleted && u.Id == dto.Id, true);
            if(existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {dto.Id} not found.");
            }
            var updateUser = _mapper.Map<User>(dto);
            updateUser.ModifiedDate = DateTime.Now;
            //Only update user's information, not password
            //If update password, then create new hash and salt
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                updateUser.Password = passwordHash.passwordHash;
                updateUser.PasswordSalt = passwordHash.salt;
            }

            await _userRepository.UpdateAsync(updateUser);
            return true;
        }
        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            ArgumentNullException.ThrowIfNull(id, $"{nameof(id)} is null");
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "User ID must be greater than zero.");
            }
            var user = await _userRepository.GetAsync(u => !u.IsDeleted && u.Id == id, true);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            //Soft delete
            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
