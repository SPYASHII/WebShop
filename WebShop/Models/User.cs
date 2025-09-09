using ProtoBuf;

namespace WebShop.Models
{
    [ProtoContract]
    public class User
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Login { get; set; }
        [ProtoMember(3)]
        public string Password { get; set; }
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public User(string login)
        {
            Login = login;
        }
        public User() { }
    }
}
