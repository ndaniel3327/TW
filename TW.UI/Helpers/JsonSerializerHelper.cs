using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        public static T DeserializeJsonOpenAppAssetFile<T>(string fileName) 
        {
            using var stream = Task.Run(async()=> await FileSystem.Current.OpenAppPackageFileAsync(fileName)).Result;
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            return JsonSerializer.Deserialize<T>(json);
        }

    }
}
