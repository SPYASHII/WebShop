using ProtoBuf;
using WebShop.Models.Abstract;

namespace WebShop.Models
{
    [ProtoContract]
    public class Processor : Product
    {
        [ProtoMember(1)]
        public string Family {  get; set; }
        [ProtoMember(2)]
        public string Socket { get; set; }
        [ProtoMember(3)]
        public int Cores { get; set; }
        [ProtoMember(4)]
        public int Threads { get; set; }
        [ProtoMember(5)]
        public float ClockFrequency { get; set; }
    }
}
