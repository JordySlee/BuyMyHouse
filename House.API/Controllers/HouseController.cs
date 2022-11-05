using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using BuyMyHouse.Model.DTO;
using BuyMyHouse.BLL.Interfaces;
using System.Collections.Generic;

namespace House.API.Controllers
{
    public class HouseController
    {
        private ILogger _logger { get; }
        private IHouseService _houseService;

        public HouseController(ILogger<HouseController> log, IHouseService houseService)
        {
            _logger = log;
            _houseService = houseService;
        }

        [FunctionName("CreateHouse")]
        [OpenApiOperation(operationId: "CreateHouse", tags: new[] { "House" })]
        [OpenApiRequestBody("application/json", typeof(HouseBaseDTO), Required = true, Example = typeof(HouseBaseDTOExampleGenerator))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(string), Description = "The Created response")]
        public async Task<IActionResult> CreateHouse([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Creating a new house");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            HouseBaseDTO body;

            try
            {
                body = JsonConvert.DeserializeObject<HouseBaseDTO>(requestBody);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogInformation($"Json deserialization failed, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            try
            {
                BuyMyHouse.Model.Entity.House house = await _houseService.CreateHouse(body.Name);
                _logger.LogInformation($"House {house.HouseId} succesfully registered");
                return new CreatedAtActionResult(nameof(GetHouse), "HouseController", new { id = house.HouseId }, house);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to create house, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("GetHouse")]
        [OpenApiOperation(operationId: "GetHouse", tags: new[] { "House" })]
        [OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Id of the house")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(HouseBaseWithIdDTO), Description = "The OK response", Example = typeof(HouseBaseWithIdDTOExampleGenerator))]
        public async Task<IActionResult> GetHouse([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Getting user");
            Guid HouseId;
            BuyMyHouse.Model.Entity.House house;
            try
            {
                HouseId = Guid.Parse(req.Query["Id"]);
                house = await _houseService.GetHouse(HouseId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to get house, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            _logger.LogInformation($"User {house.HouseId} succesfully fetched");
            return new OkObjectResult(house);
        }

        [FunctionName("GetAllHouses")]
        [OpenApiOperation(operationId: "GetAllHouses", tags: new[] { "House" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<BuyMyHouse.Model.Entity.House>), Description = "The OK response")]
        public async Task<IActionResult> GetAllHouses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Getting all houses");

            try
            {
                List<BuyMyHouse.Model.Entity.House> houses = await _houseService.GetAllHouses();
                _logger.LogInformation("All houses succesfully fetched");
                return new OkObjectResult(houses);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to get all houses, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("GetAllHousesInPriceRange")]
        [OpenApiOperation(operationId: "GetAllHousesInPriceRange", tags: new[] { "House" })]
        [OpenApiParameter(name: "lowerBound", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "The lower bound for the price to search for a house")]
        [OpenApiParameter(name: "upperBound", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "The upper bound for the price to search for a house")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<BuyMyHouse.Model.Entity.House>), Description = "The OK response")]
        public async Task<IActionResult> GetAllHousesInPriceRange([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Getting all houses within bounds");

            double lowerBound;
            double upperBound;
            try
            {
                lowerBound = double.Parse(req.Query["lowerBound"]);
                upperBound = double.Parse(req.Query["upperBound"]);

                List<BuyMyHouse.Model.Entity.House> houses = await _houseService.GetAllHousesInPriceRange(lowerBound, upperBound);
                return new OkObjectResult(houses);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to get all houses within bounds, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("UpdateHouse")]
        [OpenApiOperation(operationId: "UpdateHouse", tags: new[] { "House" })]
        [OpenApiRequestBody("application/json", typeof(HouseBaseWithIdDTO), Required = true, Example = typeof(HouseBaseWithIdDTOExampleGenerator))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> UpdateHouse([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Updating house information");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            HouseBaseWithIdDTO body;

            try
            {
                body = JsonConvert.DeserializeObject<HouseBaseWithIdDTO>(requestBody);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogInformation($"Json deserialization failed, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            try
            {
                await _houseService.UpdateHouse(body.Name, body.HouseId);
                return new OkObjectResult($"The house with id: '{body.HouseId}' was updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to update house, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("DeleteHouse")]
        [OpenApiOperation(operationId: "DeleteHouse", tags: new[] { "House" })]
        [OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Id of the house")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> DeleteHouse([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("Deleting house");

            try
            {
                Guid HouseId = Guid.Parse(req.Query["Id"]);
                await _houseService.DeleteHouse(HouseId);

                return new OkObjectResult("House was succesfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to delete house, exception message: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
