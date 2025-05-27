using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AdvanceRequestController : ControllerBase
    {
        private readonly IAdvanceRequestRepository _advanceRequestRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;
        public AdvanceRequestController(IAdvanceRequestRepository advanceRequestRepository, IClientRepository clientRepository,
                                        IContractRepository contractRepository, IMapper mapper)
        {
            _advanceRequestRepository = advanceRequestRepository;
            _clientRepository = clientRepository;
            _contractRepository = contractRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet(Name = "GetAllAdvanceRequests")]
        public ActionResult GetAllAdvanceRequests([FromQuery] UrlQuery query)
        {
            var item = _advanceRequestRepository.GetAllAdvanceRequest(query);

            if (item.Data.Count == 0 || query.Page > item.Pagination.TotalPages) return NotFound();

            return Ok(item);
        }

        [Authorize]
        [HttpGet("{Id}", Name = "GetAdvanceRequest")]
        public ActionResult GetAdvanceRequest(int Id)
        {
            var advanceRequest = _advanceRequestRepository.GetAdvanceRequest(Id);
            if (advanceRequest == null) return NotFound();

            AdvanceRequestDTO advanceRequestDTO = _mapper.Map<AdvanceRequest, AdvanceRequestDTO>(advanceRequest);
            return Ok(advanceRequestDTO);
        }

        [Authorize]
        [HttpPost(Name = "PostAdvanceRequest")]
        public ActionResult PostAdvanceRequest([FromBody] AdvanceRequestDTO advanceRequest)
        {
            var client = _clientRepository.GetClientByEmail(User.Identity.Name);

            if (advanceRequest.ClientId != client.ClientId)
                return BadRequest("Cliente não pode solicitar antecipação para outro cliente");

            if (_advanceRequestRepository.PendentAdvanceRequestByClient(client.ClientId))
                return BadRequest("Cliente possui solicitação pendente.");

            _advanceRequestRepository.PostAdvanceRequest(advanceRequest);

            return CreatedAtRoute(routeName: "GetAdvanceRequest", routeValues: new { Id = advanceRequest.AdvanceRequestId }, value: advanceRequest);
        }

        [Authorize]
        [HttpPut("approve", Name = "PutAdvanceRequest")]
        public ActionResult PutAdvanceRequest([FromBody] List<AdvanceRequestDTO> advanceRequest)
        {
            foreach (var item in advanceRequest)
            {
                var obj = _advanceRequestRepository.GetAdvanceRequest(item.AdvanceRequestId);
                if (obj == null) return NotFound();
            }

            _advanceRequestRepository.PutAdvanceRequest(advanceRequest);

            _contractRepository.PutContract(advanceRequest);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{Id}", Name = "DeleteAdvanceRequest")]
        public ActionResult DeleteAdvanceRequest(int Id)
        {
            var advanceRequest = _advanceRequestRepository.GetAdvanceRequest(Id);
            if (advanceRequest == null) return NotFound();

            _advanceRequestRepository.DeleteAdvanceRequest(Id);

            return Ok();
        }
    }
}
