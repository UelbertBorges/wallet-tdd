namespace Wallet.Application.UseCases.WalletDeposit
{
    public record WalletDepositInput(string WalletId, string AuthCode, decimal Value);
}