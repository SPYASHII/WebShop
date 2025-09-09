using ProtoBuf;
using WebShop.Models.Abstract;

namespace WebShop.Models
{
    [ProtoContract]
    public class ThermalPaste : Product
    {
        [ProtoMember(1)]
        public float ThermalConductivity { get; set; }
        [ProtoMember(2)]
        public int Viscosity { get; set; }
    }
}
