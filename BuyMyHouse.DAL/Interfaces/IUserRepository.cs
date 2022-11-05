using BuyMyHouse.Model.Entity;

namespace BuyMyHouse.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUser(Guid userId);
        Task<List<User>> GetAllUsers();
        Task UpdateUser(User user);
        Task DeleteUser(Guid userId);
        Task RegisterUser(User user);
        Task<User> LoginUser(string email, string password);
        //Task LogoutUser(Guid userId);
    }
}

