using SimpleCore.Entities;

namespace SimpleCore.Identity
{
    public class IdentityProvided<TKey> : Entity<TKey>
    {
        public string Sub { get; set; }
        public string Issuer { get; set; }
        public TKey? UserId { get; set; }

        public IdentityProvided(string sub, string issuer)
        {
            Sub = sub;
            Issuer = issuer;
        }
    }
}