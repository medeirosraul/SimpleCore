namespace SimpleCore.Entities
{
    /// <summary>
    /// An Entity owned by a specific Tenant.
    /// </summary>
    /// <typeparam name="TKey">Type of the Entity Key.</typeparam>
    public class TenantEntity<TKey> : UserEntity<TKey>
    {
        /// <summary>
        /// Id of the Tenant that Entity are owned by.
        /// </summary>
        public TKey? TenantId { get; set; }
    }
}
