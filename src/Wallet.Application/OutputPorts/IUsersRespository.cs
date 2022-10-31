using Wallet.Domain;

namespace Wallet.Application.OutputPorts
{
    public interface IUsersRespository
    {
        Task<User?> FindByDocument(string document);
        Task<User> Save(User user);
    }
}
