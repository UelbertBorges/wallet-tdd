using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Wallet.API.Controllers;
using Wallet.API.Models.Requests.Users;
using Wallet.Application.UseCases;
using Wallet.Application.UseCases.CreateUser;
using Xunit;

namespace Wallet.Tests.Units.Controllers
{
    public class UsersControllerTests
    {

        [Fact]
        public async Task CreateUser_OnSuccess_ReturnsStatusCode200()
        {
            //arrange
            var mockCreateUserUseCase = new Mock<ICreateUserUseCase>();

            mockCreateUserUseCase
                .Setup(useCase => useCase.Execute(It.IsAny<CreateUserInput>()))
                .ReturnsAsync(new SuccessUseCaseOutput<CreateUserOutput>(new(Guid.NewGuid().ToString(), "authCode", 0)));

            var sut = new UsersController(mockCreateUserUseCase.Object);

            //act
            var result = (OkObjectResult)await sut.CreateUser(new CreateUserRequest("Name", "41258963211", "password123"));

            //assert
            result.StatusCode.Should().Be(200);
        }


        [Fact]
        public async Task CreateUser_OnInputConflict_ReturnsStatusCode400()
        {
            //arrange
            var mockCreateUserUseCase = new Mock<ICreateUserUseCase>();

            mockCreateUserUseCase
                .Setup(useCase => useCase.Execute(It.IsAny<CreateUserInput>()))
                .ReturnsAsync(new ErrorUseCaseOutput<CreateUserOutput>("mensagem_validacao"));

            var sut = new UsersController(mockCreateUserUseCase.Object);

            //act
            var result = (BadRequestObjectResult)await sut.CreateUser(new CreateUserRequest("Name", "41258963211", "password123"));

            //assert
            result.StatusCode.Should().Be(400);
        }


        [Fact]
        public async Task CreateUser_OnCall_InvokesCreateUserUseCaseExactlyOnce()
        {
            //arrange
            var mockCreateUserUseCase = new Mock<ICreateUserUseCase>();

            mockCreateUserUseCase
                .Setup(useCase => useCase.Execute(It.IsAny<CreateUserInput>()))
                .ReturnsAsync(new SuccessUseCaseOutput<CreateUserOutput>(new(Guid.NewGuid().ToString(), "authCode", 0)));

            var sut = new UsersController(mockCreateUserUseCase.Object);

            //act
            var result = (OkObjectResult)await sut.CreateUser(new CreateUserRequest("Name", "41258963211", "password123"));

            //assert
            mockCreateUserUseCase.Verify(useCase => useCase.Execute(It.IsAny<CreateUserInput>()), Times.Once);
        }

        [Fact]
        public async Task CreateUser_OnException_ReturnsStatusCode500()
        {
            //arrange
            var mockCreateUserUseCase = new Mock<ICreateUserUseCase>();

            mockCreateUserUseCase
                .Setup(useCase => useCase.Execute(It.IsAny<CreateUserInput>()))
                .ThrowsAsync(new Exception());

            var sut = new UsersController(mockCreateUserUseCase.Object);

            //act
            var result = (ObjectResult)await sut.CreateUser(new CreateUserRequest("Name", "41258963211", "password123"));

            //assert
            result.StatusCode.Should().Be(500);
        }
    }
}
