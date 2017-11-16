using Newtonsoft.Json;

namespace SOASerialization
{
    public abstract class JsonSerializator
    {
        public static T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
