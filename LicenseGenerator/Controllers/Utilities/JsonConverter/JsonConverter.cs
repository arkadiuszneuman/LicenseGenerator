using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LicenseGenerator.Controllers.Utilities.JsonConverter
{
    public class JsonJavascriptConverter : IJsonConverter
    {
        public string ConvertToJson(object obj)
        {
            string json =
                        JsonConvert.SerializeObject(
                          obj,
                          Formatting.Indented,
                          new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                        );

            return json;
        }
    }
}