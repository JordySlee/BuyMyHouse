using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BuyMyHouse.Model.DTO
{
    public class UserBaseDTO
    {
        [JsonRequired]
        public string FirstName { get; set; }
        [JsonRequired]
        public string LastName { get; set; }
        [JsonRequired]
        public string Email { get; set; }
        
        public string Password { get; set; }

        public double? SalaryPerYear { get; set; }

        public double? MortgageOffer { get; set; }
    }

    public class UserBaseDTOExampleGenerator : OpenApiExample<UserBaseDTO>
    {
        public override IOpenApiExample<UserBaseDTO> Build(NamingStrategy namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("User 1", new UserBaseDTO() { FirstName = "Test1", LastName = "van User1", Email = "testuser1@email.com", Password = "Secret" }, namingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("User 2", new UserBaseDTO() { FirstName = "Test2", LastName = "van User2", Email = "testuser2@email.com", Password = "Secret" }, namingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("User 3", new UserBaseDTO() { FirstName = "Test3", LastName = "van User3", Email = "testuser3@email.com", Password = "Secret" }, namingStrategy));
            return this;
        }
    }
}

