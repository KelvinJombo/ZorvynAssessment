namespace FinanceDashboard.Commons.Utilities
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static Response<T> Success(T data, string message, int statusCode = 200)
        {
            return new Response<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        public static Response<T> Failure(string message, int statusCode = 400, List<string>? errors = null)
        {
            return new Response<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}
