using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Repositories.Interfaces;
using AutoMapper;
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

        [HttpGet(Name ="GetAllContracts")]
        public ActionResult GetAllContracts([FromQuery] UrlQuery query)
        {
            var item = _contractRepository.GetAllContracts(query);

            if (item.Data.Count == 0 || query.Page > item.Pagination.TotalPages) return NotFound();

            return Ok(item);
        }

        [HttpGet(Name = "GetAllContractByClient")]
        public ActionResult GetAllContractByClient([FromQuery] int IdClient, [FromQuery] UrlQuery query)
        {
            var item = _contractRepository.GetAllContractByClient(IdClient, query);

            if (item.Data.Count == 0 || query.Page > item.Pagination.TotalPages) return NotFound();

            return Ok(item);
        }

        [HttpGet("{Id}", Name ="GetContract")]
        public ActionResult GetContract(int Id)
        {
            var contract = _contractRepository.GetContract(Id);
            if(contract == null) return NotFound();

            ContractDTO contractDTO = _mapper.Map<Contract, ContractDTO>(contract);
            return Ok(contractDTO);
        }

        [HttpPost(Name ="PostContract")]
        public ActionResult PostContract([FromBody] ContractDTO contract)
        {
            _contractRepository.PostContract(contract);

            return CreatedAtRoute(routeName: "GetContract", routeValues: new { Id = contract.ContractId}, value: contract);
        }

        [HttpPut("{Id}", Name ="PutContract")]
        public ActionResult PutContract(int Id, [FromBody] ContractDTO contract)
        {
            var obj = _contractRepository.GetContract(Id);
            if (obj == null) return NotFound();

            contract.ContractId = Id;
            _contractRepository.PutContract(contract);

            return Ok();
        }

        [HttpDelete("{Id}", Name = "DeleteContract")]
        public ActionResult DeleteContract(int Id)
        {
            var contract = _contractRepository.GetContract(Id);
            if(contract == null) return NotFound();

            _contractRepository.DeleteContract(Id);

            return NoContent();
        }
    }
}
