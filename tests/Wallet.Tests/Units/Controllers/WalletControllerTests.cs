using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wallet.API.Controllers;
using Wallet.Application.UseCases.WalletDeposit;
using Xunit;

namespace Wallet.Tests.Units.Controllers
{
    public class WalletControllerTests
    {

        [Fact]
        public async Task Deposit_OnSuccess_ReturnsStatusCode200()
        {
            //arrange
            var mockDepositUseCase = new Mock<IWalletDepositUseCase>();
            var sut = new WalletController(mockDepositUseCase.Object);

            //act
            var result = (OkObjectResult) await sut.Deposit(new("walletId", "authCode", 100m));

            //assert
            result.StatusCode.Should().Be(200);
        }


        [Fact]
        public async Task Deposit_OnCall_InvokesDepositUseCaseExcatlyOnce()
        {
            //arrange
            var mockDepositUseCase = new Mock<IWalletDepositUseCase>();

            var sut = new WalletController(mockDepositUseCase.Object);

            //act
            var result = await sut.Deposit(new("walletId", "authCode", 100m));

            //assert
            mockDepositUseCase.Verify(useCase => useCase.Execute(It.IsAny<WalletDepositInput>()), Times.Once);
        }


        [Fact]
        public async Task Deposit_OnException_ReturnsStatusCode400()
        {
            //arrange
            var mockDepositUseCase = new Mock<IWalletDepositUseCase>();
            mockDepositUseCase
                .Setup(useCase => useCase.Execute(It.IsAny<WalletDepositInput>()))
                .ThrowsAsync(new Exception());

            var sut = new WalletController(mockDepositUseCase.Object);

            //act
            var result = (BadRequestObjectResult)await sut.Deposit(new("walletId", "authCode", 100m));

            //assert
            result.StatusCode.Should().Be(400);
        }
    }
}
