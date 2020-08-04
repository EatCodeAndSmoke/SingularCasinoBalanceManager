using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CasinoBalanceManager.Application.Mediator.Pipeline {
    internal class ValidationHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> {

        private readonly IServiceProvider _provider;

        public ValidationHandler(IServiceProvider provider) {
            this._provider = provider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next) {
            var validator = _provider.GetService(typeof(IValidator<TRequest>)) as IValidator<TRequest>;
            validator?.ValidateAndThrow(request);
            return await next();
        }
    }
}
