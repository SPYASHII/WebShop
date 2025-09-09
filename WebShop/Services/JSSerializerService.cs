using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using WebShop.Interfaces;

namespace WebShop.Services
{
    public class JSSerializerService : ISerializer
    {
        public T? Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }
        public byte[] Serialize<T>(T obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes<T>(obj);
        }
    }
}
