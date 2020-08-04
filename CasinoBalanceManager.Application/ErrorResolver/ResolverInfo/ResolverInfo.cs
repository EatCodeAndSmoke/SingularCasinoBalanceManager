using CasinoBalanceManager.Core.Models;
using System;

namespace CasinoBalanceManager.Application.ErrorResolver.ResolverInfo {
    internal interface IResolverInfo<TException, TError> where TException : Exception where TError : AppError, new() {
        IResolverInfo<TException, TError> WithMessage(Func<TException, string> func);
        IResolverInfo<TException, TError> WithStackTrace(Func<TException, string> func);
        IResolverInfo<TException, TError> BindProperties(Action<TException, TError> action);
    }

    internal class ResolverInfo<TException, TError> : BaseResolverInfo, IResolverInfo<TException, TError> 
        where TException : Exception
        where TError : AppError, new() {

        private Func<TException, string> _messageFunc, _stackTraceFunc;
        private Action<TException, TError> _bindAction;

        public ResolverInfo(int errorCode) : base(errorCode) {

        }

        public IResolverInfo<TException, TError> BindProperties(Action<TException, TError> action) {
            _bindAction = action;
            return this;
        }

        public IResolverInfo<TException, TError> WithMessage(Func<TException, string> func) {
            _messageFunc = func;
            return this;
        }

        public IResolverInfo<TException, TError> WithStackTrace(Func<TException, string> func) {
            _stackTraceFunc = func;
            return this;
        }

        protected override void BindProperties(Exception exception, AppError appError) {
            _bindAction?.Invoke(exception as TException, appError as TError);
        }

        protected override AppError GetErrorInstance() {
            return new TError();
        }

        protected override string GetMessage(Exception exception) {
            return _messageFunc != null ? _messageFunc.Invoke(exception as TException) : base.GetMessage(exception);
        }

        protected override string GetStackTrace(Exception exception) {
            return _stackTraceFunc != null ? _stackTraceFunc.Invoke(exception as TException) : base.GetStackTrace(exception);
        }

    }
}
