namespace Wallet.API.Models.Requests.Wallets
{
    public record DepositWalletRequest(string WalletId, string AuthCode, decimal Value);
}
