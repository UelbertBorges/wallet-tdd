using Microsoft.AspNetCore.Mvc;
using Wallet.API.Models.Requests.Wallets;
using Wallet.Application.UseCases.WalletDeposit;

namespace Wallet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletDepositUseCase _walletDepositUseCase;

        public WalletController(IWalletDepositUseCase walletDepositUseCase)
        {
            _walletDepositUseCase = walletDepositUseCase;
        }

        [HttpPost(Name = "Deposit")]
        public async Task<IActionResult> Deposit(DepositWalletRequest request)
        {
            try
            {
                return Ok(await _walletDepositUseCase.Execute(new(request.WalletId, request.AuthCode, request.Value)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
