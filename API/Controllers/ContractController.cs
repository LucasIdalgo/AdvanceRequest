using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;
        public ContractController(IContractRepository contractRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _mapper = mapper;
        }


        [HttpGet(Name = "GetAllContracts")]
        public ActionResult GetAllContracts([FromQuery] UrlQuery query)
        {
            var item = _contractRepository.GetAllContracts(query);

            if (item.Data == null || item.Data.Count == 0 || query.Page > item.Pagination.TotalPages)
                return NotFound(new ResponseDefaultAll<List<ContractDTO>>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Contratos não encontrados",
                    Pagination = item.Pagination
                });

            item.Status = Ok().StatusCode.ToString();
            item.Message = "Sucesso";

            return Ok(item);
        }


        [HttpGet("byClient", Name = "GetAllContractByClient")]
        public ActionResult GetAllContractByClient([FromQuery] int IdClient, [FromQuery] UrlQuery query)
        {
            var item = _contractRepository.GetAllContractByClient(IdClient, query);

            if (item.Data == null || item.Data.Count == 0 || query.Page > item.Pagination.TotalPages)
                return NotFound(new ResponseDefaultAll<List<ContractDTO>>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Contratos não encontrados",
                    Pagination = item.Pagination
                });

            item.Status = Ok().StatusCode.ToString();
            item.Message = "Sucesso";

            return Ok(item);
        }


        [HttpGet("{Id}", Name = "GetContract")]
        public ActionResult GetContract(int Id)
        {
            var contract = _contractRepository.GetContract(Id);
            if (contract == null)
                return NotFound(new ResponseDefault<ContractDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Contrato não encontrado"
                });

            ContractDTO contractDTO = _mapper.Map<Contract, ContractDTO>(contract);
            return Ok(new ResponseDefault<ContractDTO>
            {
                Data = contractDTO,
                Status = Ok().StatusCode.ToString(),
                Message = "Sucesso"
            });
        }


        [HttpPost(Name = "PostContract")]
        public ActionResult PostContract([FromBody] ContractDTO contract)
        {
            _contractRepository.PostContract(contract);

            return CreatedAtRoute(routeName: "GetContract", routeValues: new { Id = contract.ContractId }, value: contract);
        }


        [HttpPut("{Id}", Name = "PutContract")]
        public ActionResult PutContract(int Id, [FromBody] ContractDTO contract)
        {
            var obj = _contractRepository.GetContract(Id);
            if (obj == null)
                return NotFound(new ResponseDefault<ContractDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Contrato não encontrado"
                });

            contract.ContractId = Id;
            _contractRepository.PutContract(contract);

            return Ok(new ResponseDefault<ContractDTO>
            {
                Data = contract,
                Status = Ok().StatusCode.ToString(),
                Message = "Contrato alterado com sucesso"
            });
        }


        [HttpDelete("{Id}", Name = "DeleteContract")]
        public ActionResult DeleteContract(int Id)
        {
            var contract = _contractRepository.GetContract(Id);
            if (contract == null)
                return NotFound(new ResponseDefault<ContractDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Contrato não encontrado"
                });

            _contractRepository.DeleteContract(Id);

            return NoContent();
        }
    }
}
