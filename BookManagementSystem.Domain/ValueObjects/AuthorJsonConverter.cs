using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookManagementSystem.Domain.ValueObjects
{
    public class AuthorJsonConverter : JsonConverter<Author>
    {
        
        public override Author? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.TryGetInt32(out var value);
            return new Author(value, null);
        }

        public override void Write(Utf8JsonWriter writer, Author value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}