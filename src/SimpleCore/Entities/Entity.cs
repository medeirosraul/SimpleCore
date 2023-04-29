namespace SimpleCore.Entities
{
    /// <summary>
    /// A base entity.
    /// </summary>
    /// <typeparam name="TKey">Type of Entity Key.</typeparam>
    public abstract class Entity<TKey>
    {
        /// <summary>
        /// Entity identificator.
        /// </summary>
        public virtual TKey Id { get; set; } = default!;

        /// <summary>
        /// When entity created date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last entity modification date.
        /// </summary>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// If entity is deleted.
        /// </summary>
        public bool Deleted { get; set; }
    }
}