using FluentAssertions;
using Moq;
using Wallet.Application.OutputPorts;
using Wallet.Application.UseCases;
using Wallet.Application.UseCases.WalletDeposit;
using Wallet.Tests.Fixtures;
using Xunit;

namespace Wallet.Tests.Units.UseCases
{
    public class WalletDepositUseCaseTests
    {

        [Fact]
        public async Task WalletDeposit_WhenWalletNotExistsInDatabase_ReturnsErrorOuputWithValidationMessage()
        {
            //arrange
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockWalletsRepository
                .Setup(repo => repo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Wallet?)null);

            var sut = new WalletDepositUseCase(mockWalletsRepository.Object);
            var expected = new ErrorUseCaseOutput<WalletDepositOutput>("A carteira informada não foi encontrada para depósito.");

            //act
            var result = await sut.Execute(new(Guid.NewGuid().ToString(), "45684", 100m));

            //assert
            result.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task WalletDeposit_WhenAuthCodeDoesNotMatch_ReturnsErrorOutputWithValidationMessage()
        {
            //arrange
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockWalletsRepository
                .Setup(repo => repo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(WalletsFixture.GetWallet());

            var sut = new WalletDepositUseCase(mockWalletsRepository.Object);
            var expected = new ErrorUseCaseOutput<WalletDepositOutput>("Código de autorização inválido.");

            //act
            var result = await sut.Execute(new(Guid.NewGuid().ToString(), "1235", 100m));

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task WalletDeposit_WhenDepositMade_InvokesSaveWalletInDatabase()
        {
            //arrange
            var wallet = WalletsFixture.GetWallet();
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockWalletsRepository
                .Setup(repo => repo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(wallet);
            mockWalletsRepository
                .Setup(repo => repo.Save(It.IsAny<Domain.Wallet>()))
                .ReturnsAsync(wallet);

            var sut = new WalletDepositUseCase(mockWalletsRepository.Object);
            var expected = new SuccessUseCaseOutput<WalletDepositOutput>(new(wallet.Id.ToString(), wallet.Balance));

            //act
            var result = await sut.Execute(new(Guid.NewGuid().ToString(), wallet.AuthCode, 100m));

            //assert
            mockWalletsRepository.Verify(repo => repo.Save(It.IsAny<Domain.Wallet>()), Times.Once);
        }

        [Fact]
        public async Task WalletDeposit_WhenExistsWalletAndAuthCodeMatch_ReturnSuccessAndBalanceShouldBeIncremeted()
        {
            //arrange
            var wallet = WalletsFixture.GetWallet();
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockWalletsRepository
                .Setup(repo => repo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(wallet);
            mockWalletsRepository
                .Setup(repo => repo.Save(It.IsAny<Domain.Wallet>()))
                .ReturnsAsync(wallet);

            var sut = new WalletDepositUseCase(mockWalletsRepository.Object);
            var expected = new SuccessUseCaseOutput<WalletDepositOutput>(new(wallet.Id.ToString(), wallet.Balance + 100));

            //act
            var result = await sut.Execute(new(Guid.NewGuid().ToString(), wallet.AuthCode, 100m));

            //assert
            result.Should().BeEquivalentTo(expected, options => options.Excluding(o => o.Data!.WalletId));
        }
    }
}
