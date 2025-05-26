using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IContractRepository
    {
        public List<Contract> GetAllContracts();
        public List<Contract> GetAllContractByClient(int IdCliente);
        public Contract GetContract(int Id);
        public void PostContract(Contract Contract);
        public void PutContract(Contract Contract);
        public void DeleteContract(int Id);
    }
}
