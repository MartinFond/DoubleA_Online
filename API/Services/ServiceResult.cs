namespace API.Services
{

    public class ServiceResult
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }

        private ServiceResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static ServiceResult CreateSuccess()
        {
            return new ServiceResult(true, null);
        }

        public static ServiceResult CreateError(string errorMessage)
        {
            return new ServiceResult(false, errorMessage);
        }
    }
}
