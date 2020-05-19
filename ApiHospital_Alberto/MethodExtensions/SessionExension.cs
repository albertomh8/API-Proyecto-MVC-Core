using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiHospital_Alberto.MethodExtensions
{
    public static class SessionExtensions
    {
        public static void SetObjet(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            string jsonString = session.GetString(key);
            if (jsonString == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
