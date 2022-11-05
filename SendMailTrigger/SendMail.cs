using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using BuyMyHouse.BLL.Interfaces;
using BuyMyHouse.Model.Entity;
using SendGrid.Helpers.Mail;

namespace SendMailTrigger
{
    public class SendMail
    {
        private readonly ILogger<SendMail> _logger;
        private IUserService _userService;

        public SendMail(ILogger<SendMail> log, IUserService userService)
        {
            _logger = log;
            _userService = userService;
        }

        [FunctionName("SendMailTrigger")]
        public async Task SendMailTrigger([TimerTrigger("* 30 6 * * *")] TimerInfo myTimer, ILogger log, [SendGrid(ApiKey = "SendGridApiKey")] IAsyncCollector<SendGridMessage> messageCollector)
        {
            _logger.LogInformation($"Sending mortgageOffers to all users at: {DateTime.Now}");

            List<User> users = await _userService.GetAllUsers();
            foreach (User user in users)
            {
                var message = new SendGridMessage()
                {
                    From = new EmailAddress("608452@student.inholland.nl", "BuyMyHouse"),
                    Subject = "BuyMyHouse Daily Mortgage Offer",
                    HtmlContent = $"Hello {user.FirstName} {user.LastName}, \n " +
                    $"Your <strong>yearly income</strong> has been defined as: <strong>€ {user.SalaryPerYear}</strong>. \n" +
                    $"Therefore, we can offer you a <strong>maximum mortgage</strong> of: <strong>€ {user.MortgageOffer}</strong>. \n" +
                    $"Greetings from BuyMyHouse"
                };
                message.AddTo(new EmailAddress(user.Email, ($"{user.FirstName} {user.LastName}")));

                await messageCollector.AddAsync(message);
            }

            await messageCollector.FlushAsync();
        }
    }
}
