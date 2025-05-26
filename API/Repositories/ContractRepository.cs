using API.Models;
using API.Repositories.Interfaces;

namespace API.Repositories
{
    public class ContractRepository : IContractRepository
    {
        public List<Contract> GetAllContracts()
        {
            throw new NotImplementedException();
        }

        public List<Contract> GetAllContractByClient(int IdCliente)
        {
            throw new NotImplementedException();
        }

        public Contract GetContract(int Id)
        {
            throw new NotImplementedException();
        }

        public void PostContract(Contract Contract)
        {
            throw new NotImplementedException();
        }

        public void PutContract(Contract Contract)
        {
            throw new NotImplementedException();
        }
        public void DeleteContract(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
