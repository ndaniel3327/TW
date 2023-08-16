using System.Text.Json.Serialization;
using System.Text.Json;

namespace TW.UI.Helpers
{
    public static class JsonSerializerHelper
    {
        public static T DeserializeJson<T>(string JSONValue)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<T>(JSONValue,options);
        }

    }
}
