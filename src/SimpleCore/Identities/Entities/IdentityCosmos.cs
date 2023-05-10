namespace SimpleCore.Identities.Entities
{
    /// <summary>
    /// An identity base model for CosmosDB with a property for Partition Key.
    /// </summary>
    public class IdentityCosmos : Identity
    {
        /// <summary>
        /// Partition Key
        /// </summary>
        public string UserId { get; set; }

        public IdentityCosmos()
        {
            UserId = Id;
        }
    }
}
