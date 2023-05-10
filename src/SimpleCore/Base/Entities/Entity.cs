namespace SimpleCore.Base.Entities
{
    /// <summary>
    /// A base entity.
    /// </summary>
    /// <typeparam name="TKey">Type of Entity Key.</typeparam>
    public abstract class Entity
    {
        /// <summary>
        /// Entity identificator.
        /// </summary>
        public virtual string Id { get; set; } = default!;

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

        public Entity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
            Deleted = false;
        }
    }
}