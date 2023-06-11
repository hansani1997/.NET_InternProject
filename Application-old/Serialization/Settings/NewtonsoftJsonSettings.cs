
using BlueLotus360.Com.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace BlueLotus360.Com.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}