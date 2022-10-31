namespace Wallet.Application.OutputPorts
{
    public interface IWalletsRepository
    {
        Task<Domain.Wallet> Save(Domain.Wallet wallet);
    }
}
