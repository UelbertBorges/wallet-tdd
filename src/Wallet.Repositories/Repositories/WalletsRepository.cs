using MongoDB.Driver;
using Wallet.Application.OutputPorts;

namespace Wallet.Repositories.Repositories
{
    public class WalletsRepository : IWalletsRepository
    {
        private readonly MongoContext _mongoContext;

        public WalletsRepository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<Domain.Wallet> Save(Domain.Wallet wallet)
        {
            await _mongoContext.Wallets.ReplaceOneAsync(w => w.Id == wallet.Id, wallet, new ReplaceOptions() { IsUpsert = true });
            return wallet;
        }
    }
}
