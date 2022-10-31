namespace Wallet.API.Models.Requests.Users
{
    public readonly record struct CreateUserRequest(string Name, string Document, string Password);
}
