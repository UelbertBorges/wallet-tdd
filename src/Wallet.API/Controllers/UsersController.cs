using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Requests.Users;
using Wallet.Application.UseCases.CreateUser;

namespace Wallet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;

        public UsersController(ICreateUserUseCase createUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            try
            {
                var output = await _createUserUseCase.Execute(new(request.Name, request.Document, request.Password));
                return output.Success ? Ok(output) : BadRequest(output);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
