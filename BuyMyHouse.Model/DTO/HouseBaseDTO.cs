using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BuyMyHouse.Model.DTO
{
    public class HouseBaseDTO
    {
        [JsonRequired]
        public string Name { get; set; }
    }

    public class HouseBaseDTOExampleGenerator : OpenApiExample<HouseBaseDTO>
    {
        public override IOpenApiExample<HouseBaseDTO> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("House 1", new HouseBaseDTO() { Name = "House 1" }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("House 2", new HouseBaseDTO() { Name = "House 2" }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("House 3", new HouseBaseDTO() { Name = "House 3" }, NamingStrategy));
            return this;
        }
    }

}
