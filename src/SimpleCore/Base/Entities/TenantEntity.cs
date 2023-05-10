namespace SimpleCore.Base.Entities
{
    /// <summary>
    /// An Entity owned by a specific Tenant.
    /// </summary>
    /// <typeparam name="TKey">Type of the Entity Key.</typeparam>
    public class TenantEntity : UserEntity
    {
        /// <summary>
        /// Id of the Tenant that Entity are owned by.
        /// </summary>
        public string? TenantId { get; set; }
    }
}
