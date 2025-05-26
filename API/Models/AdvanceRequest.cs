using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AdvanceRequest
    {
        [Key]
        public int AdvanceRequestId { get; set; }

        [ForeignKey("Contract")]
        public int ContractId { get; set; }

        [Required]
        public int InstallmentQuantity { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required]
        public bool Approve { get; set; }

        public DateTime? ApprovedAt { get; set; }
        
        [Required]        
        public DateTime CreatedAt { get; set; }
    }
}
