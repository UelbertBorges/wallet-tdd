namespace Wallet.Domain
{
    public sealed class Wallet
    {
        public Guid Id { get; private set; }
        public Guid Owner { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime CreatedAt { get; set; }

        private Wallet(Guid id, Guid owner, decimal balance)
        {
            Id = id;
            Owner = owner;
            Balance = balance;
        }

        public static Wallet CreateWallet(Guid owner, decimal balance = 0)
        {
            return new Wallet(Guid.NewGuid(), owner, balance)
            {
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
