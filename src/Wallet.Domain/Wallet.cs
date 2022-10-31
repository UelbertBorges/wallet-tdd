namespace Wallet.Domain
{
    public sealed class Wallet
    {
        public Guid Id { get; private set; }
        public Guid Owner { get; private set; }
        public decimal Balance { get; private set; }
        public string AuthCode { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Wallet(Guid id, Guid owner, decimal balance, string authCode)
        {
            Id = id;
            Owner = owner;
            Balance = balance;
            AuthCode = authCode;
        }

        public static Wallet CreateWallet(Guid owner, string authCode, decimal balance = 0)
        {
            return new Wallet(Guid.NewGuid(), owner, balance, authCode)
            {
                CreatedAt = DateTime.UtcNow,
            };
        }

        public void Deposit(decimal value)
        {
            if (value > 0)
                Balance += value;
        }
    }
}
