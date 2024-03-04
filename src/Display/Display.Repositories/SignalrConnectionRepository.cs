using Display.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Display.Models.App;

namespace Display.Repositories
{
    public class SignalrConnectionRepository : ISignalRRepository
    {
        private readonly MongoClient _client;
        private readonly string CollectionName = "SignalrConnections";

        public SignalrConnectionRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);
        }

        private IMongoCollection<SignalrConnectionModel> GetTenantCollection(string tenantId)
        {
            var database = _client.GetDatabase(tenantId);
            return database.GetCollection<SignalrConnectionModel>(CollectionName);
        }
        public async Task<IEnumerable<SignalrConnectionModel>> GetByFilterAsync(string tenantId, FilterDefinition<SignalrConnectionModel> filter)
        {
            var collection = GetTenantCollection(tenantId);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task AddAsync(string tenantId, SignalrConnectionModel model)
        {
            var collection = GetTenantCollection(tenantId);
            await collection.InsertOneAsync(model);
        }

        public async Task<bool> RemoveAsync(string tenantId, SignalrConnectionModel model)
        {
            var collection = GetTenantCollection(tenantId);
            var deleteResult = await collection.DeleteOneAsync(x => x.Id == model.Id);
            return deleteResult.IsAcknowledged;
        }
    }
}
