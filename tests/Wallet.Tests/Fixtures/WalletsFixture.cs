namespace Wallet.Tests.Fixtures
{
    public static class WalletsFixture
    {
        public static Domain.Wallet GetWallet()
        {
            return Domain.Wallet.CreateWallet(UsersFixture.GetUser().Id, "authCode", 0m);
        }
    }
}
