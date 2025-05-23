namespace CollegeApp.Services
{
    public interface IUserService
    {
        string CreatePasswordHash(string password);
    }
}
