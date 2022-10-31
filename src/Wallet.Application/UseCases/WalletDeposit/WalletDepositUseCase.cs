using Wallet.Application.OutputPorts;

namespace Wallet.Application.UseCases.WalletDeposit
{
    public class WalletDepositUseCase : IWalletDepositUseCase
    {
        private IWalletsRepository _walletsRepository;

        public WalletDepositUseCase(IWalletsRepository walletsRepository)
        {
            _walletsRepository = walletsRepository;
        }

        public async Task<UseCaseOutput<WalletDepositOutput>> Execute(WalletDepositInput input)
        {
            Domain.Wallet? wallet = await _walletsRepository.FindById(Guid.Parse(input.WalletId));

            if (wallet == null)
                return new ErrorUseCaseOutput<WalletDepositOutput>("A carteira informada não foi encontrada para depósito.");
            
            if (wallet!.AuthCode != input.AuthCode)
                return new ErrorUseCaseOutput<WalletDepositOutput>("Código de autorização inválido.");

            wallet.Deposit(input.Value);
            wallet = await _walletsRepository.Save(wallet);

            return new SuccessUseCaseOutput<WalletDepositOutput>(new(wallet.Id.ToString(), wallet.Balance));
        }
    }
}
