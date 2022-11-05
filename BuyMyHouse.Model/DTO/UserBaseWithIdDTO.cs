using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace BuyMyHouse.Model.DTO
{
    public class UserBaseWithIdDTO : UserBaseDTO
    {        
        public Guid UserId { get; set; }
    }


    public class UserBaseWithIdDTOExampleGenerator : OpenApiExample<UserBaseWithIdDTO>
    {
        public override IOpenApiExample<UserBaseWithIdDTO> Build(NamingStrategy namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("User 1", new UserBaseWithIdDTO() { UserId = Guid.NewGuid(), FirstName = "Test1", LastName = "van User1", Email = "testuser1@email.com", Password = "Secret" }, namingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("User 2", new UserBaseWithIdDTO() { UserId = Guid.NewGuid(), FirstName = "Test2", LastName = "van User2", Email = "testuser2@email.com", Password = "Secret" }, namingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("User 3", new UserBaseWithIdDTO() { UserId = Guid.NewGuid(), FirstName = "Test3", LastName = "van User3", Email = "testuser3@email.com", Password = "Secret" }, namingStrategy));
            return this;
        }
    }
}

