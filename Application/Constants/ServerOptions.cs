using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Constants
{
    public static class ServerOptions
    {
        public static readonly JsonSerializerSettings JsonSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            }
        };
    }
}
