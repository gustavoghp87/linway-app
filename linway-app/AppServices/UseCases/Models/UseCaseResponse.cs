namespace AppServices.UseCases
{
    public class UseCaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public static UseCaseResponse Ok() =>
            new UseCaseResponse()
            {
                Success = true
            };
        public static UseCaseResponse Fail(string errorMessage) =>
            new UseCaseResponse()
            {
                Success = false,
                ErrorMessage = errorMessage
            };
    }

    public class UseCaseResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public static UseCaseResponse<T> Ok(T data = default) =>
            new UseCaseResponse<T>() {
                Success = true,
                Data = data
            };
        public static UseCaseResponse<T> Fail(string errorMessage) =>
            new UseCaseResponse<T>() {
                Success = false,
                ErrorMessage = errorMessage
            };
    }
}
