using API.Models.DTO;
using API.Models;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Repositories.Interfaces
{
    public interface IContractRepository
    {
        public ResponseAll<ContractDTO> GetAllContracts(UrlQuery query);
        public ResponseAll<ContractDTO> GetAllContractByClient(int IdClient, UrlQuery query);
        public Contract GetContract(int Id);
        public void PostContract(ContractDTO Contract);
        public void PutContract(ContractDTO Contract);
        public void PutContract(List<AdvanceRequestDTO> advanceRequests);
        public void DeleteContract(int Id);
    }
}
