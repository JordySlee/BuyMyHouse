using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using BuyMyHouse.BLL.Interfaces;

namespace UpdateMortgageTrigger
{
    public class UpdateMortgage
    {
        private readonly ILogger<UpdateMortgage> _logger;
        private IUserService _userService;

        public UpdateMortgage(ILogger<UpdateMortgage> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("UpdateMortgageTrigger")]
        public async Task UpdateMortgageTrigger([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
        {
            _logger.LogInformation($"Updating mortgages for all users at: {DateTime.Now}");

            await _userService.UpdateMortgageAllUsers();

            _logger.LogInformation($"Next mortgage update is scheduled at: {myTimer.ScheduleStatus.Next}");
        }
    }
}
