using CasinoBalanceManager.Core.Models;
using System;

namespace CasinoBalanceManager.Application.ErrorResolver.ResolverInfo {
    internal abstract class BaseResolverInfo {

        private readonly int _errorCode;

        public BaseResolverInfo(int errorCode) {
            _errorCode = errorCode;
        }

        protected abstract AppError GetErrorInstance();
        protected abstract void BindProperties(Exception exception, AppError appError);

        protected virtual string GetMessage(Exception exception) {
            return exception.Message;
        }

        protected virtual string GetStackTrace(Exception exception) {
            return exception.StackTrace;
        }

        internal virtual AppError GetError<TException>(TException exception) where TException : Exception {
            var targetError = GetErrorInstance();
            targetError.ErrorCode = _errorCode;
            string errorMessage = GetMessage(exception);
            string stackTrace = GetStackTrace(exception);
            targetError.Message = !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : "Exception has no message";
            targetError.StackTrace = !string.IsNullOrWhiteSpace(stackTrace) ? stackTrace : "Exception has no stackTrace";
            BindProperties(exception, targetError);
            return targetError;
        }


    }
}
