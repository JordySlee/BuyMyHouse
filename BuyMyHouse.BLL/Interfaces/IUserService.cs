using BuyMyHouse.Model.DTO;
using BuyMyHouse.Model.Entity;


namespace BuyMyHouse.BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserBaseWithIdDTO> RegisterUser(UserBaseDTO userBase);
        Task<UserBaseDTO> GetUser(Guid userId);
        Task<List<User>> GetAllUsers();
        Task UpdateUser(UserBaseWithIdDTO userBaseWithId);
        Task DeleteUser(Guid userId);
        Task UpdateMortgageAllUsers();
    }
}

