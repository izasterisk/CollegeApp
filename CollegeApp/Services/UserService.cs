using CollegeApp.Data.Repository;
using CollegeApp.Data;
using AutoMapper;

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
        public string CreatePasswordHash(string password)
        {
            return null;
        }
    }
}
