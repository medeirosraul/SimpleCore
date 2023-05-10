using SimpleCore.Base.Entities;

namespace SimpleCore.Identities.Entities;

public class IdentityProvided : Entity
{
    public string Sub { get; set; }
    public string Issuer { get; set; }
    public string? UserId { get; set; }

    public IdentityProvided(string sub, string issuer)
    {
        Sub = sub;
        Issuer = issuer;
    }
}