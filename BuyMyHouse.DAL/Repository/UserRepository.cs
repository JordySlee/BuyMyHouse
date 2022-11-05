using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Model.Entity;
using BuyMyHouse.DAL.Interfaces;

namespace BuyMyHouse.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BuyMyHouseContext _buyMyHouseContext;
        public UserRepository(BuyMyHouseContext buyMyHouseContext)
        {
            _buyMyHouseContext = buyMyHouseContext;
        }

        public async Task RegisterUser(User user)
        {
            await _buyMyHouseContext.Users.AddAsync(user);
            await _buyMyHouseContext.SaveChangesAsync();
        }

        public async Task<User?> GetUser(Guid userId)
        {
            return await _buyMyHouseContext.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _buyMyHouseContext.Users.ToListAsync();
        }


        public async Task UpdateUser(User newUser)
        {
            try
            {
                User? oldUser = await _buyMyHouseContext.Users.FindAsync(newUser.UserId);

                if (oldUser != null && oldUser.DeletedAt == null)
                {
                    _buyMyHouseContext.Entry(oldUser).CurrentValues.SetValues(newUser);
                    await _buyMyHouseContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public async Task DeleteUser(Guid userId)
        {
            try
            {
                User? oldUser = await _buyMyHouseContext.Users.FindAsync(userId);

                if (oldUser != null && oldUser.DeletedAt == null)
                {
                    User newUser = oldUser;
                    newUser.DeletedAt = DateTime.Now;
                    _buyMyHouseContext.Entry(oldUser).CurrentValues.SetValues(newUser);
                    await _buyMyHouseContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public Task<User> LoginUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task LogoutUser(Guid userId)
        {
            throw new NotImplementedException();
        }

    }
}

