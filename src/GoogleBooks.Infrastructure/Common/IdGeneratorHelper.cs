using MongoDB.Bson;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure.Common;

public class IdGeneratorHelper(IMongoDatabase database)
{
    private readonly IMongoCollection<BsonDocument> _counterCollection = database.GetCollection<BsonDocument>("counters");

    public int GetNextId(string entityName)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", entityName);
        var update = Builders<BsonDocument>.Update.Inc("Seq", new BsonInt32(1));

        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };

        var result = _counterCollection.FindOneAndUpdate(filter, update, options);
        return result["Seq"].AsInt32;
    }
}
