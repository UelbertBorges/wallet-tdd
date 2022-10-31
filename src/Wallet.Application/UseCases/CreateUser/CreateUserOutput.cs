namespace Wallet.Application.UseCases.CreateUser
{
    public record CreateUserOutput(string WalletId, string AuthorizationCode, decimal Balance);
}