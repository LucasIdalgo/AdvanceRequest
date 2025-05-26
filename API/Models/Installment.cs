using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Installment
    {
        [Key]
        public int InstallmentId { get; set; }

        [ForeignKey("Contract")]
        public int ContractId { get; set; }

        public Contract? Contract { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public bool Antecipated { get; set; }
    }
}
