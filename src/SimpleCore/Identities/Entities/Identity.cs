using SimpleCore.Base.Entities;

namespace SimpleCore.Identities.Entities;

public class Identity : Entity
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool IsValidUserInfo { get; set; }

    public Identity()
    {

    }
}