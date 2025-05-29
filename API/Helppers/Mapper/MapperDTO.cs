using API.Models;
using API.Models.DTO;
using AutoMapper;

namespace API.Helppers.Mapper
{
    public class MapperDTO : Profile
    {
        public MapperDTO()
        {
            CreateMap<Client, ClientDTO>();
            CreateMap<ClientDTO, Client>();
            CreateMap<Contract, ContractDTO>();
            CreateMap<ContractDTO, Contract>();
            CreateMap<Installment, InstallmentDTO>();
            CreateMap<InstallmentDTO, Installment>();
            CreateMap<AdvanceRequest, AdvanceRequestDTO>();
            CreateMap<AdvanceRequestDTO, AdvanceRequest>();
        }
    }
}
