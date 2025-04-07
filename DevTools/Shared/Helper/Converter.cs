using System.Text.Json;

namespace DevTools.Helper.Converter
{
    public static class Converter
    {
        public static object ConvertJsonElement(object input)
        {
            if (input is JsonElement jsonElement)
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.String:
                        return jsonElement.GetString();   // Chuyển về string
                    case JsonValueKind.Number:
                        if (jsonElement.TryGetInt32(out int intValue))
                            return intValue;             // Nếu là số nguyên -> int
                        if (jsonElement.TryGetDouble(out double doubleValue))
                            return doubleValue;         // Nếu là số thực -> double
                        return jsonElement.GetDecimal(); // Nếu cần hỗ trợ decimal
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        return jsonElement.GetBoolean(); // Chuyển về bool
                    case JsonValueKind.Object:
                        return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonElement.GetRawText());
                    case JsonValueKind.Array:
                        return JsonSerializer.Deserialize<List<object>>(jsonElement.GetRawText());
                    case JsonValueKind.Null:
                        return null;
                    default:
                        return jsonElement.GetRawText(); // Trả về JSON dạng string nếu không xác định được
                }
            }
            return input;
        }
    }
}