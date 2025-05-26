namespace API.Models.DTO
{
    public class ClientDTO
    {
        public int clientId { get; set; }
        public required string name { get; set; }
        public required string doc { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
    }
}
