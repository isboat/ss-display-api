using Display.Models;
using Display.Models.App;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Display.Repositories
{
    public class DeviceRegistrationRepository : IDeviceRegistrationRepository
    {
        private readonly MongoClient _client;
        private readonly string CollectionName = "DeviceCodeRegistration";
        private readonly string DbName = "device-app-db";

        public DeviceRegistrationRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);
        }

        public async Task<DeviceCodeRegistrationModel> GetAsync(string deviceCode)
        {
            var database = _client.GetDatabase(DbName);
            var collection = database.GetCollection<DeviceCodeRegistrationModel>(CollectionName);
            var model = await (await collection.FindAsync(x => x.DeviceCode == deviceCode)).FirstOrDefaultAsync();

            return model;
        }

        public async Task<DeviceCodeRegistrationModel?> GetAsyncById(string deviceId)
        {
            var database = _client.GetDatabase(DbName);
            var collection = database.GetCollection<DeviceCodeRegistrationModel>(CollectionName);
            var model = await (await collection.FindAsync(x => x.Id == deviceId)).FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> InsertAsync(DeviceCodeRegistrationModel model)
        {
            var database = _client.GetDatabase(DbName);
            var collection = database.GetCollection<DeviceCodeRegistrationModel>(CollectionName);
            await collection.InsertOneAsync(model);

            return true;
        }
    }
}
