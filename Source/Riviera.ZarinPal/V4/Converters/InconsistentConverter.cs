namespace Riviera.ZarinPal.V4
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Converts empty array to null.
    /// </summary>
    internal class InconsistentConverter : JsonConverter<object?>
    {
        /// <inheritdoc/>
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                _ = reader.Read();
                return default;
            }

            return JsonSerializer.Deserialize(ref reader, typeToConvert, options);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, object? objectToWrite, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }
    }
}
