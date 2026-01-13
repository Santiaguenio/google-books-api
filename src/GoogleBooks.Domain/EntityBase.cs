namespace GoogleBooks.Domain;

public class EntityBase<TKey>
{
    public TKey Id { get; set; } = default!;

    public DateTime CreationDate { get; set; }
    public DateTime LastUpdate { get; set; }
}