namespace API.Models.Responses
{
    public class ResponseAll<T>
    {
        public List<T> Data { get; set; }
        public Pagination Pagination { get; set; }
    }
}
