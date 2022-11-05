using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BuyMyHouse.Model.DTO
{
    public class HouseBaseWithIdDTO : HouseBaseDTO
    {
        [JsonRequired]
        public Guid HouseId { get; set; }
    }

    public class HouseBaseWithIdDTOExampleGenerator : OpenApiExample<HouseBaseWithIdDTO>
    {
        public override IOpenApiExample<HouseBaseWithIdDTO> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("House 1", new HouseBaseWithIdDTO() { HouseId = Guid.NewGuid(), Name = "House 1" }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("House 2", new HouseBaseWithIdDTO() { HouseId = Guid.NewGuid(), Name = "House 2" }, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("House 3", new HouseBaseWithIdDTO() { HouseId = Guid.NewGuid(), Name = "House 3" }, NamingStrategy));
            return this;
        }
    }

}
