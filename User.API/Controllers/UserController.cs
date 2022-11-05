using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using BuyMyHouse.Model.DTO;
using BuyMyHouse.BLL.Interfaces;


namespace User.API.Controllers
{
    public class UserController
    {
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;

        public UserController(ILogger<UserController> log, IUserService _userService)
        {
            _logger = log;
            this._userService = _userService;
        }

        [FunctionName("RegisterUser")]
        [OpenApiOperation(operationId: "RegisterUser", tags: new[] { "User" })]
        [OpenApiRequestBody("application/json", typeof(UserBaseDTO), Required = true, Example = typeof(UserBaseDTOExampleGenerator), Description = "User data.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(string), Description = "The Created response")]
        public async Task<IActionResult> RegisterUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Registering a new user");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserBaseDTO body;
            try
            {
                body = JsonConvert.DeserializeObject<UserBaseDTO>(requestBody);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogInformation($"Json deserialization failed, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            try
            {
                UserBaseWithIdDTO user = await _userService.RegisterUser(body);
                _logger.LogInformation($"User {user.UserId} succesfully registered");
                return new CreatedAtActionResult(nameof(GetUser), "UserController", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to register user, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("GetUser")]
        [OpenApiOperation(operationId: "GetUser", tags: new[] { "User" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The Id of the user.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserBaseWithIdDTO), Description = "The OK response with the retrieved user.", Example = typeof(UserBaseWithIdDTOExampleGenerator))]
        public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Getting user");
            Guid userId;
            UserBaseDTO user;
            try
            {
                userId = Guid.Parse(req.Query["userId"]);
                user = await _userService.GetUser(userId);                
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to get user, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            _logger.LogInformation($"User {userId} succesfully fetched");
            return new OkObjectResult(user);
        }

        [FunctionName("UpdateUser")]
        [OpenApiOperation(operationId: "UpdateUser", tags: new[] { "User" })]
        [OpenApiRequestBody("application/json", typeof(UserBaseWithIdDTO), Description = "User data.", Example = typeof(UserBaseWithIdDTOExampleGenerator))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Updating user information.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();           
            UserBaseWithIdDTO body;
            try
            {
                body = JsonConvert.DeserializeObject<UserBaseWithIdDTO>(requestBody);               
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogInformation($"Json deserialization failed, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            try
            {
                await _userService.UpdateUser(body);
                return new OkObjectResult($"User's information was updated");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to update user information, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("DeleteUser")]
        [OpenApiOperation(operationId: "DeleteUser", tags: new[] { "User" })]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The Id of the user.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Deleting user");
            try
            {
                Guid userId = Guid.Parse(req.Query["userId"]);
                await _userService.DeleteUser(userId);

                return new OkObjectResult($"User was succesfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to delete user, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

}

