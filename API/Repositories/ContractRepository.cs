﻿using API.DataBase;
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

        public ResponseDefaultAll<List<ContractDTO>> GetAllContracts(UrlQuery query)
        {
            var retorno = new ResponseDefaultAll<List<ContractDTO>>();
            var item = _db.Contract.AsNoTracking().AsQueryable();

            var pagination = new Pagination
            {
                Page = query.Page,
                LimitByPages = query.Limit,
                Total = item.Count(),
                TotalPages = item.Count() > 0 ? (int)Math.Ceiling((double)item.Count() / query.Limit) : 0
            };

            item = item.Skip((query.Page - 1) * query.Limit).Take(query.Limit);

            retorno.Pagination = pagination;
            retorno.Data = item.Count() > 0 ? _mapper.Map<List<Contract>, List<ContractDTO>>(item.ToList()) : null;

            return retorno;
        }

        public ResponseDefaultAll<List<ContractDTO>> GetAllContractByClient(int IdClient, UrlQuery query)
        {
            var retorno = new ResponseDefaultAll<List<ContractDTO>>();
            var item = _db.Contract.AsNoTracking().Where(c => c.ClientId == IdClient).AsQueryable();

            var pagination = new Pagination
            {
                Page = query.Page,
                LimitByPages = query.Limit,
                Total = item.Count(),
                TotalPages = item.Count() > 0 ? (int)Math.Ceiling((double)item.Count() / query.Limit) : 0
            };

            item = item.Skip((query.Page - 1) * query.Limit).Take(query.Limit);

            retorno.Pagination = pagination;
            retorno.Data = item.Count() > 0 ? _mapper.Map<List<Contract>, List<ContractDTO>>(item.ToList()) : null;

            if (retorno.Data != null)
                foreach (var contract in retorno.Data)
                {
                    var item2 = _db.Installment.AsNoTracking().Where(i => i.ContractId == contract.ContractId && i.Status=="open").AsQueryable();
                    contract.Installments = _mapper.Map<List<Installment>, List<InstallmentDTO>>(item2.ToList());
                }

            return retorno;
        }

        public Contract GetContract(int Id)
        {
            var contract = _db.Contract.AsNoTracking().FirstOrDefault(c => c.ContractId == Id);
            contract.Installments = _db.Installment.AsNoTracking().Where(i => i.ContractId == Id).ToList();

            return contract;
        }

        public void PostContract(ContractDTO Contract)
        {
            var contract = _mapper.Map<ContractDTO, Contract>(Contract);
            _db.Contract.Add(contract);
            _db.SaveChanges();

            foreach (var installment in Contract.Installments)
            {
                installment.ContractId = contract.ContractId;
                _db.Installment.Add(_mapper.Map<InstallmentDTO, Installment>(installment));
            }

            Contract = _mapper.Map<Contract, ContractDTO>(contract);

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

                for (int i = 0; i < advanceRequest.InstallmentQuantity; i++)
                {
                    contract.Installments[contract.Installments.Count - i].Antecipated = true;
                    contract.Installments[contract.Installments.Count - i].Status = "paid";
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
