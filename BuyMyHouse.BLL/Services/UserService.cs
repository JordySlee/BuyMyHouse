using BuyMyHouse.BLL.Interfaces;
using BuyMyHouse.Model.DTO;
using BuyMyHouse.DAL.Interfaces;
using BuyMyHouse.Model.Entity;

namespace BuyMyHouse.BLL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly double MortgageMultiplier = 4.4;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserBaseWithIdDTO> RegisterUser(UserBaseDTO userBase)
        {
            User user = new()
            {
                UserId = Guid.NewGuid(),
                FirstName = userBase.FirstName,
                LastName = userBase.LastName,
                Email = userBase.Email,
                ImageUrl = null,
                DeletedAt = null,
            };

             await _userRepository.RegisterUser(user);

            UserBaseWithIdDTO userBaseWithIdDTO = new()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };
            return userBaseWithIdDTO;
        }

        public async Task<UserBaseDTO> GetUser(Guid userId)
        {          
            User? user = await _userRepository.GetUser(userId);
            UserBaseDTO dto = new()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return dto;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task UpdateUser(UserBaseWithIdDTO userBaseWithIdDTO)
        {
            User user = new()
            {
                UserId = userBaseWithIdDTO.UserId,
                FirstName = userBaseWithIdDTO.FirstName,
                LastName = userBaseWithIdDTO.LastName,
                Email = userBaseWithIdDTO.Email,
            };
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(Guid userId)
        {
            await _userRepository.DeleteUser(userId);
        }

        public async Task UpdateMortgageAllUsers()
        {
            List<User> users = await GetAllUsers();
            foreach (User user in users)
            {
                user.MortgageOffer = CalculateMortgageOffer(user.SalaryPerYear);
                await _userRepository.UpdateUser(user);
            }
        }

        public double CalculateMortgageOffer(double? SalaryPerYear)
        {
            /*
             
            Er is gebruik gemaakt van de volgende website:
            https://dehypotheeksite.nl/hypotheekinformatie/maximale-hypotheek/hoe-wordt-je-maximale-hypotheek-berekend.php
            "Voorbeeld maximaal hypotheekbedrag zonder lening
            Stel dat je maximale hypotheeklast € 1.347 is, zoals in het voorgaande voorbeeld.
            Bij een looptijd van 30 jaar en een toetsrente van 4,5% is jouw maximale hypotheek € 265.861.
            Dit is 4,4 keer je bruto jaarinkomen."


            Voor de eenvoudigheid wordt standaard gekozen voor de volgende opzet (zelfde als in het voorbeeld):
            - Annuïteiten verzekering
            - Rentevaste periode van 30 jaar
            - Rentepercentage van 4.5%

            Dat betekent dat de hypotheekaanvraag versimpeld kan worden naar:
            Maximale hypotheekbedrag = jaarlijkse bruto inkomen * 4,4
            */
            double mortgageOffer = 0;
            if (SalaryPerYear != null)
            {
                mortgageOffer = (double)SalaryPerYear * MortgageMultiplier;
            }

            return mortgageOffer;
        }
    }
}

