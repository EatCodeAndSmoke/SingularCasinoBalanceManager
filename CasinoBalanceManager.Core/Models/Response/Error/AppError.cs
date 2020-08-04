using CasinoBalanceManager.Core.Exceptions;

namespace CasinoBalanceManager.Core.Models {
    public class AppError {

        private string _message, _stackTrace;

        public AppError() {

        }

        public AppError(int errorCode, string message, string stackTrace) {
            ErrorCode = errorCode;
            Message = message;
            StackTrace = stackTrace;
        }

        public int ErrorCode { get; set; }

        public string Message {
            get {
                return _message;
            }

            set {
                ThrowIfEmptyString(nameof(Message), value);
                _message = value;
            }
        }

        public string StackTrace {
            get {
                return _stackTrace;
            }

            set {
                ThrowIfEmptyString(nameof(StackTrace), value);
                _stackTrace = value;
            }
        }

        private void ThrowIfEmptyString(string propName, string value) {
            if (string.IsNullOrWhiteSpace(value))
                throw new CoreModelException($"{propName} property of {typeof(AppError).Name} class is required");
        }
    }
}
