using System.Text.Json;
using BlueLotus360.Com.Application.Interfaces.Serialization.Options;

namespace BlueLotus360.Com.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}