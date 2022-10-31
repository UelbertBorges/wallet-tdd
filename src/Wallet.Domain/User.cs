namespace Wallet.Domain
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Document { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User(string name, string document, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Document = document;
            Password = password;
        }

        public static User CreateUser(string name, string document, string password)
        {
            return new User(name, document, password)
            {
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}