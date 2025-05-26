namespace API.Models.DTO
{
    public class ContractDTO
    {
        public int ContractId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public List<InstallmentDTO> Installments { get; set; } = new List<InstallmentDTO>();
    }
}
