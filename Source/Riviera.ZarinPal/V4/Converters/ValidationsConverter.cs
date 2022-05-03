namespace Riviera.ZarinPal.V4
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Converts the validation errors to a dictionaty.
    /// </summary>
    internal class ValidationsConverter : JsonConverter<List<KeyValuePair<string, string?>>>
    {
        /// <inheritdoc/>
        public override List<KeyValuePair<string, string?>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }

            var items = new List<KeyValuePair<string, string?>>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    continue;
                }

                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return items;
                }

                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException();
                }

                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                string? propertyName = reader.GetString();
                if (string.IsNullOrEmpty(propertyName))
                {
                    throw new JsonException();
                }

                reader.Read();
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException();
                }

                string? propertyValue = reader.GetString();

                items.Add(new KeyValuePair<string, string?>(propertyName, propertyValue));
            }

            return items;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, List<KeyValuePair<string, string?>> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }
    }
}
