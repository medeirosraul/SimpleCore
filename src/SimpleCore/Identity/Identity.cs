using SimpleCore.Entities;

namespace SimpleCore.Identity
{
    public class Identity<TKey> : Entity<TKey>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsValidUserInfo { get; set; }

        public Identity()
        {
            
        }
    }
}