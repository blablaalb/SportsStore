using System.Text.Json;
using System.Text;

namespace SportsStore.Infrastructure;

public static class SessionExtensions
{
    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetJson<T>(this ISession session, string key)
    {
        var sessionData = session.GetString(key);
        T sessionDataDeserialized = sessionData == null ? default (T) : JsonSerializer.Deserialize<T>(sessionData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false
        });
        return sessionDataDeserialized;
    }
}
