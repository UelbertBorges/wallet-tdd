using Wallet.Domain;

namespace Wallet.Tests.Fixtures
{
    public static class UsersFixture
    {
        public static User GetUser() => User.CreateUser("Name", "12345678912", "password123");
    }
}
