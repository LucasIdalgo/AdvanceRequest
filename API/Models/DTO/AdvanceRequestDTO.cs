namespace API.Models.DTO
{
    public class AdvanceRequestDTO
    {
        public int AdvanceRequestId { get; set; }
        public int ContractId { get; set; }
        public int InstallmentQuantity { get; set; }
        public int ClientId { get; set; }
        public bool Approve { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
