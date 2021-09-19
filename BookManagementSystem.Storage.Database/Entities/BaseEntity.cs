namespace BookManagementSystem.Storage.Database.Entities
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }

    }
}