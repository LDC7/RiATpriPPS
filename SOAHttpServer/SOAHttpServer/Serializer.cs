using System.Text;
using Newtonsoft.Json;

namespace Serializer
{ 
    public class Serializer : ISerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            return Encoding.UTF8.GetBytes((string) JsonConvert.SerializeObject(obj));
        }

        public T Deserialize<T>(byte[] source)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(source));
        }
    }
}