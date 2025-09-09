using ProtoBuf;
using WebShop.Models.Abstract;

namespace WebShop.Models
{
    [ProtoContract]
    public class Cart
    {
        [ProtoMember(1)]
        public Dictionary<int, CartItem> IdProductQtyPairs { get; set; } = new Dictionary<int, CartItem>();
    }
}
