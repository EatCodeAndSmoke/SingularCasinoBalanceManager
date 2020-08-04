using CasinoBalanceManager.Application.Mediator.Commands.CasinoBalance;
using FluentValidation;

namespace CasinoBalanceManager.Application.Validation {
    internal class BaseBalanceCommandValidator<TBalanceCommand> : AbstractValidator<TBalanceCommand> 
        where TBalanceCommand : BaseBalanceCommand {
        public BaseBalanceCommandValidator() {
            RuleFor(m => m.Amount)
                .GreaterThan(0)
                .WithMessage($"{nameof(BaseBalanceCommand.Amount)} must be greater than 0");

            RuleFor(m => m.TransactionId)
                .NotEmpty()
                .WithMessage($"{nameof(BaseBalanceCommand.TransactionId)} must be specified");
        }
    }
}
