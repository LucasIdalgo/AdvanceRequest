using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Doc { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; }= string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
