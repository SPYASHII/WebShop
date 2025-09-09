using WebShop.Interfaces;
using ProtoBuf.Serializers;
using ProtoBuf;

namespace WebShop.Services
{
    public class ProtoSerializer : ISerializer
    {
        public T? Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
                return Serializer.Deserialize<T>(ms);
        }

        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, obj);

                return ms.ToArray();
            }
        }
    }
}
