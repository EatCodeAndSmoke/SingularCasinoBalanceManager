using System;
using System.Collections.Generic;
using System.Linq;

namespace CasinoBalanceManager.Application.Common.Exceptions {
    public class TransactionFailedException : Exception {

        private readonly Queue<TransactionExecutionLog> _logQueue;

        public TransactionFailedException() {
            _logQueue = new Queue<TransactionExecutionLog>();
        }

        public override string Message => $"Transaction failed. See {nameof(ExecutionLog)} property for more information";
        internal IEnumerable<TransactionExecutionLog> ExecutionLog => _logQueue;
        internal bool HasErrors => ExecutionLog.Any(l => !l.Succeed);

        internal void LogDecreaseSuccess(string targetBalance) {
            _logQueue.Enqueue(TransactionExecutionLog.DecreaseSuccess(targetBalance));
        }

        internal void LogDecreaseFailure(string targetBalance, string errorCode) {
            string msg = FailMessage(targetBalance, TransactionExecutionLog.ExecutionLogType.Decrease, errorCode);
            _logQueue.Enqueue(TransactionExecutionLog.DecreaseFailed(targetBalance, msg));
        }

        internal void LogIncreaseSuccess(string targetBalance) {
            _logQueue.Enqueue(TransactionExecutionLog.IncreaseSuccess(targetBalance));
        }

        internal void LogIncreaseFailure(string targetBalance, string errorCode) {
            string msg = FailMessage(targetBalance, TransactionExecutionLog.ExecutionLogType.Increase, errorCode);
            _logQueue.Enqueue(TransactionExecutionLog.IncreaseFailed(targetBalance, msg));
        }

        internal void LogRollbackSuccess(string targetBalance) {
            _logQueue.Enqueue(TransactionExecutionLog.RollbackSuccess(targetBalance));
        }

        internal void LogRollbackFailure(string targetBalance, string errorCode) {
            string msg = FailMessage(targetBalance, TransactionExecutionLog.ExecutionLogType.Rollback, errorCode);
            _logQueue.Enqueue(TransactionExecutionLog.RollbackFailed(targetBalance, msg));
        }

        private string FailMessage(string targetBalance, TransactionExecutionLog.ExecutionLogType LogType, string errorCode) {
            string logTypeStr = LogType.ToString().ToLower();
            return $"Failed to {logTypeStr} {targetBalance} balance. Reason: {errorCode}";
        }
    }

    public class TransactionExecutionLog {

        private TransactionExecutionLog() {

        }

        public string TargetBalance { get; private set; }
        public ExecutionLogType LogType { get; private set; }
        public bool Succeed { get; private set; }
        public string ErrorMessage { get; private set; }

        internal static TransactionExecutionLog DecreaseSuccess(string targetBalance) {
            return Create(targetBalance, ExecutionLogType.Decrease, true, null);
        }

        internal static TransactionExecutionLog DecreaseFailed(string targetBalance, string errorMessage) {
            return Create(targetBalance, ExecutionLogType.Decrease, false, errorMessage);
        }

        internal static TransactionExecutionLog IncreaseSuccess(string targetBalance) {
            return Create(targetBalance, ExecutionLogType.Increase, true, null);
        }

        internal static TransactionExecutionLog IncreaseFailed(string targetBalance, string errorMessage) {
            return Create(targetBalance, ExecutionLogType.Increase, false, errorMessage);
        }

        internal static TransactionExecutionLog RollbackSuccess(string targetBalance) {
            return Create(targetBalance, ExecutionLogType.Rollback, true, null);
        }

        internal static TransactionExecutionLog RollbackFailed(string targetBalance, string errorMessage) {
            return Create(targetBalance, ExecutionLogType.Rollback, false, errorMessage);
        }

        private static TransactionExecutionLog Create(string targetBalance, ExecutionLogType logType, bool succeed, string errorMessage) {
            return new TransactionExecutionLog {
                ErrorMessage = errorMessage,
                LogType = logType,
                Succeed = succeed,
                TargetBalance = targetBalance
            };
        }

        public enum ExecutionLogType {
            Decrease,
            Increase,
            Rollback
        }
    }

}
