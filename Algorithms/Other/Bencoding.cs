using System;
using System.Text;
using System.Text.Json;

namespace Algorithms.Other
{
    public static class Bencoding
    {
        public static string Encode(JsonElement element)
        {
            if(element.ValueKind == JsonValueKind.Number)
            {
                return EncodeInt(element);
            }
            else if(element.ValueKind == JsonValueKind.String)
            {
                return EncodeString(ref element);
            }
            else if(element.ValueKind == JsonValueKind.Array)
            {
                return EncodeArray(ref element);
            }
            else if(element.ValueKind == JsonValueKind.Object)
            {
                return EncodeDic(ref element);
            }
            else
            {
                throw new ArgumentException("Element is not a valid type", nameof(element));
            }
        }

        private static string EncodeInt(JsonElement element)
        {
            return $"i{element.GetInt64()}e";
        }

        private static string EncodeString(ref JsonElement element)
        {
            string? value = element.GetString();
            if (value != null)
            {
                return $"{value.Length}:{value}";
            }
            else
            {
                throw new ArgumentException("Element is not a string or is null", nameof(element));
            }
        }

        private static string EncodeArray(ref JsonElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('l');
            foreach (var item in element.EnumerateArray())
            {
                builder.Append(Encode(item));
            }

            builder.Append('e');
            return builder.ToString();
        }

        private static string EncodeDic(ref JsonElement element)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('d');
            foreach (var item in element.EnumerateObject())
            {
                string name = item.Name;
                JsonDocument doc = JsonDocument.Parse(name);
                JsonElement json = doc.RootElement.Clone();
                _ = builder.Append(EncodeString(ref json));
                builder.Append(Encode(item.Value));
            }

            builder.Append('e');
            return builder.ToString();
        }
    }
}
