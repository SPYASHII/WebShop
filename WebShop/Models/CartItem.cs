using ProtoBuf;
using WebShop.Models.Abstract;

namespace WebShop.Models
{
    [ProtoContract]
    public class CartItem
    {
        [ProtoMember(1)]
        public Product Product { get; set; }
        [ProtoMember(2)]
        public int Qty { get; set; }

        public CartItem() { }
        public CartItem(Product product, int qty) 
        {
            Product = product;
            Qty = qty;
        }
    }
}
