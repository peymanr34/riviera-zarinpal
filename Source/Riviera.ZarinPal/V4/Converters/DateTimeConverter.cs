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

            throw new FormatException("The JSON value is not in a supported DateTime format.");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
