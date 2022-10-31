using FluentAssertions;
using Moq;
using Wallet.Application.OutputPorts;
using Wallet.Application.UseCases;
using Wallet.Application.UseCases.CreateUser;
using Wallet.Domain;
using Wallet.Tests.Fixtures;
using Xunit;

namespace Wallet.Tests.Units.UseCases
{
    public class CreateUserUseCaseTests
    {

        [Fact]
        public async Task CreateUser_WhenDocumentAlreadyExists_ReturnsErrorOutputWithValidationMessage()
        {
            //arrange
            var mockUsersRepository = new Mock<IUsersRespository>();
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockUsersRepository
                .Setup(repo => repo.FindByDocument(It.IsAny<string>()))
                .ReturnsAsync(User.CreateUser("Name", "12345678912", "password123"));

            var expected = new ErrorUseCaseOutput<CreateUserOutput>("Já existe usuário cadastrado com esses dados");

            var sut = new CreateUserUseCase(mockUsersRepository.Object, mockWalletsRepository.Object);

            //act
            var result = await sut.Execute(new("Name", "12345678912", "password123"));

            //assert
            result.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task CreateUser_WhenDocumentNotExistsInDatabase_InvokesSaveUserInDatabaseExactlyOnce()
        {
            //arrange
            var mockUsersRepository = new Mock<IUsersRespository>();
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockUsersRepository
                .Setup(repo => repo.FindByDocument(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            mockUsersRepository
                .Setup(repo => repo.Save(It.IsAny<User>()))
                .ReturnsAsync(UsersFixture.GetUser());
            mockWalletsRepository
                .Setup(repo => repo.Save(It.IsAny<Domain.Wallet>()))
                .ReturnsAsync(WalletsFixture.GetWallet());

            var sut = new CreateUserUseCase(mockUsersRepository.Object, mockWalletsRepository.Object);

            //act
            var result = await sut.Execute(new("Name", "12345678912", "password123"));

            //assert
            mockUsersRepository.Verify(repo => repo.Save(It.IsAny<User>()), Times.Once);
        }


        [Fact]
        public async Task CreateUser_WhenUserIsCreated_InvokesSaveWalletInDatabaseExactlyOnce()
        {
            //arrange
            var mockUsersRepository = new Mock<IUsersRespository>();
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockUsersRepository
                .Setup(repo => repo.FindByDocument(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            mockUsersRepository
                .Setup(repo => repo.Save(It.IsAny<User>()))
                .ReturnsAsync(UsersFixture.GetUser());
            mockWalletsRepository
                .Setup(repo => repo.Save(It.IsAny<Domain.Wallet>()))
                .ReturnsAsync(WalletsFixture.GetWallet());

            var sut = new CreateUserUseCase(mockUsersRepository.Object, mockWalletsRepository.Object);

            //act
            var result = await sut.Execute(new("Name", "12345678912", "password123"));

            //assert
            mockWalletsRepository.Verify(repo => repo.Save(It.IsAny<Domain.Wallet>()), Times.Once);
        }


        [Fact]
        public async Task CreateUser_WhenUserAndWalletIsCreated_ReturnsSuccessOutputWithData()
        {
            //arrange
            var mockUsersRepository = new Mock<IUsersRespository>();
            var mockWalletsRepository = new Mock<IWalletsRepository>();

            mockUsersRepository
                .Setup(repo => repo.FindByDocument(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            mockUsersRepository
                .Setup(repo => repo.Save(It.IsAny<User>()))
                .ReturnsAsync(UsersFixture.GetUser());
            mockWalletsRepository
                .Setup(repo => repo.Save(It.IsAny<Domain.Wallet>()))
                .ReturnsAsync(Domain.Wallet.CreateWallet(UsersFixture.GetUser().Id, 0));

            var expected = new SuccessUseCaseOutput<CreateUserOutput>(new("guid", "code", 0));

            var sut = new CreateUserUseCase(mockUsersRepository.Object, mockWalletsRepository.Object);

            //act
            var result = await sut.Execute(new("Name", "12345678912", "password123"));

            //assert
            result.Should().BeEquivalentTo(expected, options => options.Excluding(o => o.Data!.WalletId)
                                                                       .Excluding(o => o.Data!.AuthorizationCode));
        }
    }
}
