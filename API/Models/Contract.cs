using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        public bool Active { get; set; }

        public List<Installment> Installments { get; set; } = new List<Installment>();
    }   
}
