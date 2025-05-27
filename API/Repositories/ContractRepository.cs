using API.DataBase;
using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DesafioContext _db;
        private readonly IMapper _mapper;
        public ContractRepository(DesafioContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public ResponseAll<ContractDTO> GetAllContracts(UrlQuery query)
        {
            var retorno = new ResponseAll<ContractDTO>();
            var item = _db.Contract.AsNoTracking().AsQueryable();

            var pagination = new Pagination
            {
                Page = query.Page,
                LimitByPages = query.Limit,
                Total = item.Count(),
                TotalPages = (int)Math.Ceiling((double)item.Count() / query.Limit)
            };

            item = item.Skip((query.Page - 1) * query.Limit).Take(query.Limit);

            retorno.Pagination = pagination;
            retorno.Data = _mapper.Map<List<Contract>, List<ContractDTO>>(item.ToList());

            return retorno;
        }

        public ResponseAll<ContractDTO> GetAllContractByClient(int IdClient, UrlQuery query)
        {
            var retorno = new ResponseAll<ContractDTO>();
            var item = _db.Contract.AsNoTracking().Where(c => c.ClientId == IdClient).AsQueryable();

            var pagination = new Pagination
            {
                Page = query.Page,
                LimitByPages = query.Limit,
                Total = item.Count(),
                TotalPages = (int)Math.Ceiling((double)item.Count() / query.Limit)
            };

            item = item.Skip((query.Page - 1) * query.Limit).Take(query.Limit);

            retorno.Pagination = pagination;
            retorno.Data = _mapper.Map<List<Contract>, List<ContractDTO>>(item.ToList());

            return retorno;
        }

        public Contract GetContract(int Id)
        {
            return _db.Contract.AsNoTracking().FirstOrDefault(c => c.ContractId == Id);
        }

        public void PostContract(ContractDTO Contract)
        {
            _db.Contract.Add(_mapper.Map<ContractDTO, Contract>(Contract));

            foreach (var installment in Contract.Installments)
                _db.Installment.Add(_mapper.Map<InstallmentDTO, Installment>(installment));

            _db.SaveChanges();
        }

        public void PutContract(ContractDTO Contract)
        {
            _db.Contract.Update(_mapper.Map<ContractDTO, Contract>(Contract));

            foreach (var installment in Contract.Installments)
                _db.Installment.Update(_mapper.Map<InstallmentDTO, Installment>(installment));

            _db.SaveChanges();
        }

        public void PutContract(List<AdvanceRequestDTO> advanceRequests)
        {
            foreach (var advanceRequest in advanceRequests)
            {
                var contract = GetContract(advanceRequest.ContractId);

                for(int i = contract.Installments.Count; i > advanceRequest.InstallmentQuantity; i--)
                {
                    contract.Installments[i].Antecipated = true;
                    contract.Installments[i].Status = "paid";
                }

                foreach (var installment in contract.Installments)
                    _db.Installment.Update(installment);
            }

            _db.SaveChanges();
        }

        public void DeleteContract(int Id)
        {
            var contract = GetContract(Id);
            contract.Active = false;

            _db.Contract.Update(contract);
            _db.SaveChanges();
        }
    }
}
