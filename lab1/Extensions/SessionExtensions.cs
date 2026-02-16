using System.Text.Json;
using System.Text.Json.Serialization;

namespace lab1.Extensions
{
    public static class SessionExtensions
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value, _options);
            session.SetString(key, json);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            if (string.IsNullOrEmpty(json)) return default;
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        public static void RemoveKey(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}