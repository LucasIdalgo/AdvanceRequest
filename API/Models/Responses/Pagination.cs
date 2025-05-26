namespace API.Models.Responses
{
    public class Pagination
    {
        public int Page { get; set; }
        public int LimitByPages { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }
}
