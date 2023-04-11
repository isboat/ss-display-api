using Display.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Display.Repositories
{
    public class ScreenRepository : IRepository<ScreenModel>
    {
        private readonly MongoClient _client;
        private readonly string CollectionName = "Screens";

        public ScreenRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);
        }

        private IMongoCollection<ScreenModel> GetTenantScreenCollection(string tenantId)
        {
            var database = _client.GetDatabase(tenantId);
            return database.GetCollection<ScreenModel>(CollectionName);
        }

        public async Task<List<ScreenModel>> GetAllByTenantIdAsync(string tenantId)
        {
            return await GetTenantScreenCollection(tenantId).Find(x => x.TenantId == tenantId).ToListAsync();
        }

        public async Task<ScreenModel?> GetAsync(string tenantId, string id)
        {
            return await GetTenantScreenCollection(tenantId).Find(x => x.Id == id && x.TenantId == tenantId).FirstOrDefaultAsync();
        }
    }
}
