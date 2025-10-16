using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure;

public static class MongoDbConfiguration
{
    private static bool _isConfigured = false;

    public static void ConfigureConventions()
    {
        if (_isConfigured) return;

        // Register global conventions
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(), // Optional: Convert property names to camelCase
            new EnumRepresentationConvention(BsonType.String), // Optional: Enums as strings
            new IgnoreExtraElementsConvention(true) // Optional: Ignore extra elements in documents
        };

        ConventionRegistry.Register("GlobalConventions", conventionPack, _ => true);

        // Register the custom serializer for GUIDs to use Standard format
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));

        _isConfigured = true;
    }

    public static IMongoClient CreateClient(string connectionString)
    {
        ConfigureConventions();

        var settings = MongoClientSettings.FromConnectionString(connectionString);
        return new MongoClient(settings);
    }
}
