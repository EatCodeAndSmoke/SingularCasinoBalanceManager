using CasinoBalanceManager.Application.Mediator;
using CasinoBalanceManager.Application.Mediator.Commands.CasinoBalance;
using CasinoBalanceManager.Application.Mediator.Queries.CasinoBalance;
using CasinoBalanceManager.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CasinoBalanceManager.Api.Controllers {
    [Route("api")]
    [ApiController]
    public class CasinoController : ControllerBase {

        private readonly IAppMediator appMediator;

        public CasinoController(IAppMediator appMediator) {
            this.appMediator = appMediator;
        }

        [HttpGet("balance")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AppError), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBalance() {
            var result = await appMediator.Execute(new GetCasinoBalanceQuery());
            return result.Succeed ? Ok(result.Body.Value) : StatusCode((int)HttpStatusCode.InternalServerError, result.Error);
        }

        [HttpPost("deposit/{transactionId}/{amount}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AppError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(AppError), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Deposit([FromRoute]string transactionId, [FromRoute]decimal amount) {
            return await ExecuteBalanceCommand<DepositCommand>(transactionId, amount);
        }

        [HttpPost("withdraw/{transactionId}/{amount}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AppError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(AppError), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Withdraw([FromRoute]string transactionId, [FromRoute] decimal amount) {
            return await ExecuteBalanceCommand<WithdrawCommand>(transactionId, amount);
        }

        private async Task<IActionResult> ExecuteBalanceCommand<TCommand>(string transactionId, decimal amount) where TCommand : BaseBalanceCommand, new() {
            var command = new TCommand {
                Id = HttpContext.TraceIdentifier,
                TransactionId = transactionId,
                Amount = amount
            };

            var result = await appMediator.Execute(command);
            if (result.Succeed)
                return Ok();

            return result.Error.ErrorCode == 101 ? BadRequest(result.Error)
                                                 : StatusCode((int)HttpStatusCode.InternalServerError, result.Error);
        }
    }
}
