using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookManagementSystem.Domain.ValueObjects
{
    public class CategoryJsonConverter : JsonConverter<Category>
    {
        public override Category? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.TryGetInt32(out int value);
            return new Category(value, null);
        }

        public override void Write(Utf8JsonWriter writer, Category value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}