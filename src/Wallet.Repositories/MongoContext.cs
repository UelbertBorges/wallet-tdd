using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using Wallet.Repositories.Conventions;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Wallet.Domain;

namespace Wallet.Repositories
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(string connectionString, string dataBase)
        {
            RegisterConvention();
            MappingEntity();
            MongoClient _mongoClient = new(connectionString);
            _database = _mongoClient.GetDatabase(dataBase);
        }

        private static void RegisterConvention() => ConventionRegistry.Register("ConventionConfig", new ConventionPack() {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true),
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
                new GuidAsStringRepresentationConvention()
        }, t => true);

        private static void MappingEntity()
        {
            MapUser();
            MapWallet();
        }

        private static void MapUser()
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.GetMemberMap(u => u.CreatedAt)
                   .SetSerializer(new DateTimeSerializer(BsonType.DateTime));
            });
        }

        private static void MapWallet()
        {
            BsonClassMap.RegisterClassMap<Domain.Wallet>(map =>
            {
                map.AutoMap();
                map.GetMemberMap(w => w.Balance).SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                map.GetMemberMap(m => m.CreatedAt)
                   .SetSerializer(new DateTimeSerializer(BsonType.DateTime));
            });
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("User");
        public IMongoCollection<Domain.Wallet> Wallets => _database.GetCollection<Domain.Wallet>("Wallet");
    }
}
