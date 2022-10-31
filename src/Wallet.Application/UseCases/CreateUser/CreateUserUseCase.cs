using Wallet.Application.OutputPorts;
using Wallet.Domain;
using System.Text.RegularExpressions;

namespace Wallet.Application.UseCases.CreateUser
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly Random _random;
        private readonly IUsersRespository _usersRespository;
        private readonly IWalletsRepository _walletsRepository;

        public CreateUserUseCase(IUsersRespository usersRespository, IWalletsRepository walletsRepository)
        {
            _usersRespository = usersRespository;
            _walletsRepository = walletsRepository;
            _random = new();
        }

        public async Task<UseCaseOutput<CreateUserOutput>> Execute(CreateUserInput input)
        {
            if (await ExistsUserWithDocument(input.Document))
                return new ErrorUseCaseOutput<CreateUserOutput>("Já existe usuário cadastrado com esses dados");

            User user = await SaveUserInDatabase(input);
            Domain.Wallet wallet = await SaveWalletInDatabase(user);

            return GetSuccessOutput(wallet);
        }

        private SuccessUseCaseOutput<CreateUserOutput> GetSuccessOutput(Domain.Wallet wallet)
        {
            string walletId = wallet.Id.ToString();
            return new(new CreateUserOutput(walletId, GenerateAuthorizationCode(walletId), wallet.Balance));
        }

        private string GenerateAuthorizationCode(string source)
        {
            Regex rgx = new("[^a-zA-Z0-9]");
            string chars = rgx.Replace(source, "");

            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        private async Task<Domain.Wallet> SaveWalletInDatabase(User user)
        {
            return await _walletsRepository.Save(Domain.Wallet.CreateWallet(user.Id, GenerateAuthorizationCode(user.Id.ToString())));
        }

        private async Task<User> SaveUserInDatabase(CreateUserInput input)
        {
            return await _usersRespository.Save(User.CreateUser(input.Name, input.Document, input.Password));
        }

        private async Task<bool> ExistsUserWithDocument(string document)
        {
            User? user = await _usersRespository.FindByDocument(document);
            return user != null;
        }
    }
}
