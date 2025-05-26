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
            CreateMap<Contract, ContractDTO>();
            CreateMap<Installments, InstallmentsDTO>();
            CreateMap<AdvanceRequest, AdvanceRequestDTO>();
        }
    }
}
