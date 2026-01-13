using MongoDB.Driver;

namespace GoogleBooks.Infrastructure.Readers.Persistence;

internal class ReaderListByCriteriaStrategy
{
    public static Collation GetStrategy() => new("fr", strength: CollationStrength.Primary);
}
