using Display.Models.App;
using Display.Repositories;
using MongoDB.Driver;

namespace Display.Services
{
    public class SignalrConnectionService : ISignalrConnectionService
    {
        private readonly ISignalRRepository _repository;

        public SignalrConnectionService(ISignalRRepository repository)
        {
            _repository = repository;
        }

        public Task AddAsync(string tenantId, SignalrConnectionModel model) =>
            _repository.AddAsync(tenantId, model);

        public Task<IEnumerable<SignalrConnectionModel>> GetByFilterAsync(string tenantId, FilterDefinition<SignalrConnectionModel> filter) =>
            _repository.GetByFilterAsync(tenantId, filter);

        public Task<bool> RemoveAsync(string tenantId, SignalrConnectionModel model) =>
            _repository.RemoveAsync(tenantId, model);
    }
}
