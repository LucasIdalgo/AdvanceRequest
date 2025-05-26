using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace API.Models.DTO
{
    public class ContractDTO
    {
        public int contractId { get; set; }
        public required int clientId { get; set; }
        public required string clientName { get; set; }
        [JsonIgnore]
        public bool active { get; set; }
        public required ObservableCollection<Installments> installments { get; set; }
    }

    public class InstallmentsDTO
    {
        public int installmentId { get; set; }
        public int contractId { get; set; }
        public DateTime dueDate { get; set; }
        public double amount { get; set; }
        public required string status { get; set; }
        public bool antecipated { get; set; }
    }
}
