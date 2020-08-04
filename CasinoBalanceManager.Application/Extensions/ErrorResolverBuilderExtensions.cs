using CasinoBalanceManager.Application.Common.Exceptions;
using CasinoBalanceManager.Application.ErrorResolver;
using CasinoBalanceManager.Application.Models;
using CasinoBalanceManager.Core.Models;
using FluentValidation;
using System;
using System.Text;

namespace CasinoBalanceManager.Application.Extensions {
    internal static class ErrorResolverBuilderExtensions {

        internal static IErrorResolverBuilder AddApplicationErrors(this IErrorResolverBuilder builder) {
            builder.Register<TransactionFailedException, TransactionFailedErrorModel>(errorCode: 100)
                .BindProperties((ex, er) => {
                    er.ExecutionLog = ex.ExecutionLog;
                });

            builder.Register<ValidationException, AppError>(errorCode: 101)
                .WithMessage(e => {
                    StringBuilder strBuilder = new StringBuilder();
                    foreach (var item in e.Errors)
                        strBuilder.AppendLine(item.ToString());

                    return strBuilder.ToString();
                });

            builder.Register<Exception, AppError>(errorCode: 102);

            return builder;
        }
    }
}
