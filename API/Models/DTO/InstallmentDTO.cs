namespace API.Models.DTO
{
    public class InstallmentDTO
    {
        public int InstallmentId { get; set; }
        public int ContractId { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool Antecipated { get; set; }
    }
}
