namespace SimpleCore.Entities
{
    /// <summary>
    /// An Entity owned by a specific Application User.
    /// </summary>
    /// <typeparam name="TKey">Type of the Entity Key.</typeparam>
    public class UserEntity<TKey> : Entity<TKey>
    {
        /// <summary>
        /// Id of the User that Entity are owned by.
        /// </summary>
        public TKey? OwnerId { get; set; }

        /// <summary>
        /// Id of the User that Entity are created by.
        /// </summary>
        public TKey? CreatorId { get; set; }
    }
}