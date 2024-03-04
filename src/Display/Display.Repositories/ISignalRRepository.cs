using Display.Models.App;
using MongoDB.Driver;

namespace Display.Repositories
{
    public interface ISignalRRepository
    {
        Task<IEnumerable<SignalrConnectionModel>> GetByFilterAsync(string tenantId, FilterDefinition<SignalrConnectionModel> filter);

        Task AddAsync(string tenantId, SignalrConnectionModel model);

        Task<bool> RemoveAsync(string tenantId, SignalrConnectionModel model);
    }
}
