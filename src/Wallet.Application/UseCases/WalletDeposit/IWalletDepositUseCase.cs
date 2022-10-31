namespace Wallet.Application.UseCases.WalletDeposit
{
    public interface IWalletDepositUseCase
    {
        Task<UseCaseOutput<WalletDepositOutput>> Execute(WalletDepositInput input);
    }
}
