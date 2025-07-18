namespace Riviera.ZarinPal.V4
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <inheritdoc/>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParse(reader.GetString(), out var result))
            {
                return result;
            }

            // If you throw a JsonException without a message, the serializer creates a message that
            // includes the path to the part of the JSON that caused the error. 
            // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to#error-handling
            throw new JsonException();
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
