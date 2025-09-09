using ProtoBuf;
using System.Data.SqlTypes;

namespace WebShop.Models.Abstract
{
    [ProtoContract]
    [ProtoInclude(6, typeof(Processor))]
    [ProtoInclude(7, typeof(ThermalPaste))]
    public abstract class Product
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string? Description { get; set; }
        [ProtoMember(4)]
        public decimal Price { get; set; }
        [ProtoMember(5)]
        public int Qty { get; set; }

    }
}
