using Display.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Display.Repositories
{
    public class ScreenRepository : IRepository<DetailedScreenModel>
    {
        private readonly MongoClient _client;
        private readonly string CollectionName = "PublishedScreenData";

        public ScreenRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            BsonClassMap.RegisterClassMap<TextAssetItemModel>();
            BsonClassMap.RegisterClassMap<AssetItemModel>();

            var objectSerializer = new ObjectSerializer(
                type => ObjectSerializer.DefaultAllowedTypes(type)
                || type.FullName.StartsWith("Display"));

            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        private IMongoCollection<DetailedScreenModel> GetTenantScreenCollection(string tenantId)
        {
            var database = _client.GetDatabase(tenantId);
            return database.GetCollection<DetailedScreenModel>(CollectionName);
        }

        public async Task<DetailedScreenModel?> GetAsync(string tenantId, string id)
        {
            return await GetTenantScreenCollection(tenantId).Find(x => x.Id == id && x.TenantId == tenantId).FirstOrDefaultAsync();
        }
    }
}
