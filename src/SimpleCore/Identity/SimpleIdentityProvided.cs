using SimpleCore.Entities;

namespace SimpleCore.Identity
{
    public class SimpleIdentityProvided<TKey> : Entity<TKey>
    {
        public string? Sub { get; set; }
        public string? Issuer { get; set; }
        public TKey? UserId { get; set; }
    }
}