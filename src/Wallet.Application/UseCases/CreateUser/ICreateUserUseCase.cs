namespace Wallet.Application.UseCases.CreateUser
{
    public interface ICreateUserUseCase
    {
        public Task<UseCaseOutput<CreateUserOutput>> Execute(CreateUserInput input);
    }
}
