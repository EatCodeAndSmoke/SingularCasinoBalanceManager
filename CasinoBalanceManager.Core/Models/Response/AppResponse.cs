using CasinoBalanceManager.Core.Exceptions;

namespace CasinoBalanceManager.Core.Models {
    public class AppResponse {

        private AppResponse() {

        }

        public bool Succeed { get; private set; }
        public AppResponseBody Body { get; private set; }
        public AppError Error { get; private set; }

        public static AppResponse Create(bool succeed, AppResponseBody body, AppError appError) {
            bool failedWithoutError = !succeed && appError == null;
            if (failedWithoutError)
                throw new CoreModelException("AppError object must be passed when succeed is true");

            return new AppResponse {
                Succeed = succeed,
                Body = body,
                Error = appError
            };
        }

        public static AppResponse Success(AppResponseBody body) {
            return Create(true, body, null);
        }

        public static AppResponse Success(object bodyValue) {
            return Success(new AppResponseBody(bodyValue));
        }

        public static AppResponse Failure(AppError error) {
            return Create(false, null, error);
        }

        public static AppResponse Failure(int errorCode, string errorMessage, string stackTrace) {
            return Failure(new AppError(errorCode, errorMessage, stackTrace));
        }
    }
}
