using Newtonsoft.Json;

namespace SOASerialization
{
    class JsonSer : ISerializer
    {
        public T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
