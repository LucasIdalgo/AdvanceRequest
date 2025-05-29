namespace API.Models.Responses
{
    public class ResponseDefault<T>
    {
        public T Data { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class ResponseDefaultAll<T> : ResponseDefault<T>
    {
        public Pagination Pagination { get; set; }
    }
}
