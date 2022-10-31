using MongoDB.Driver;
using Wallet.Application.OutputPorts;
using Wallet.Domain;

namespace Wallet.Repositories.Repositories
{
    public class UsersRepository : IUsersRespository
    {
        private readonly MongoContext _mongoContext;

        public UsersRepository(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<User?> FindByDocument(string document) => await _mongoContext.Users.Find(u => u.Document == document).FirstOrDefaultAsync();

        public async Task<User> Save(User user)
        {
            await _mongoContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user, new ReplaceOptions() { IsUpsert = true });
            return user;
        }
    }
}
