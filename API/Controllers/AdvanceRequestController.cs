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


        [HttpGet(Name = "GetAllAdvanceRequests")]
        public ActionResult GetAllAdvanceRequests([FromQuery] UrlQuery query)
        {
            var item = _advanceRequestRepository.GetAllAdvanceRequest(query);

            if (item.Data == null || item.Data.Count == 0 || query.Page > item.Pagination.TotalPages)
                return NotFound(new ResponseDefaultAll<List<AdvanceRequestDTO>>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Antecipações não encontradas",
                    Pagination = item.Pagination
                });

            item.Status = Ok().StatusCode.ToString();
            item.Message = "Sucesso";

            return Ok(item);
        }


        [HttpGet("{Id}", Name = "GetAdvanceRequest")]
        public ActionResult GetAdvanceRequest(int Id)
        {
            var advanceRequest = _advanceRequestRepository.GetAdvanceRequest(Id);
            if (advanceRequest == null)
                return NotFound(new ResponseDefault<AdvanceRequestDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Antecipação não encontrada"
                });

            AdvanceRequestDTO advanceRequestDTO = _mapper.Map<AdvanceRequest, AdvanceRequestDTO>(advanceRequest);
            return Ok(new ResponseDefault<AdvanceRequestDTO>
            {
                Data = advanceRequestDTO,
                Status = Ok().StatusCode.ToString(),
                Message = "Sucesso"
            });
        }


        [HttpPost(Name = "PostAdvanceRequest")]
        public ActionResult PostAdvanceRequest([FromBody] AdvanceRequestDTO advanceRequest)
        {
            var client = _clientRepository.GetClient(advanceRequest.ClientId);

            if (advanceRequest.ClientId != client.ClientId)
                return BadRequest(new ResponseDefault<AdvanceRequestDTO>
                {
                    Status = BadRequest().StatusCode.ToString(),
                    Message = "Cliente não pode solicitar antecipação para outro cliente"
                });

            if (_advanceRequestRepository.PendentAdvanceRequestByClient(client.ClientId))
                return BadRequest(new ResponseDefault<AdvanceRequestDTO>
                {
                    Status = BadRequest().StatusCode.ToString(),
                    Message = "Cliente possui antecipação pendente"
                });

            _advanceRequestRepository.PostAdvanceRequest(advanceRequest);

            return CreatedAtRoute(routeName: "GetAdvanceRequest", routeValues: new { Id = advanceRequest.AdvanceRequestId }, value: advanceRequest);
        }


        [HttpPut("approve", Name = "PutAdvanceRequest")]
        public ActionResult PutAdvanceRequest([FromBody] List<AdvanceRequestDTO> advanceRequest)
        {
            if (advanceRequest == null)
                return BadRequest(new ResponseDefault<List<AdvanceRequestDTO>>
                {
                    Status = BadRequest().StatusCode.ToString(),
                    Message = "Lista de antecipações não pode ser nulo"
                });

            foreach (var item in advanceRequest)
            {
                var obj = _advanceRequestRepository.GetAdvanceRequest(item.AdvanceRequestId);
                if (obj == null)
                    return NotFound(new ResponseDefault<AdvanceRequestDTO>
                    {
                        Status = NotFound().StatusCode.ToString(),
                        Message = "Antecipação não encontrada"
                    });
            }

            _advanceRequestRepository.PutAdvanceRequest(advanceRequest);

            _contractRepository.PutContract(advanceRequest);

            return Ok(new ResponseDefault<List<AdvanceRequestDTO>>
            {
                Data = advanceRequest,
                Status = Ok().StatusCode.ToString(),
                Message = "Antecipação aprovada com sucesso"
            });
        }


        [HttpDelete("{Id}", Name = "DeleteAdvanceRequest")]
        public ActionResult DeleteAdvanceRequest(int Id)
        {
            var advanceRequest = _advanceRequestRepository.GetAdvanceRequest(Id);
            if (advanceRequest == null)
                return NotFound(new ResponseDefault<AdvanceRequestDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Antecipação não encontrada"
                });

            _advanceRequestRepository.DeleteAdvanceRequest(Id);

            return NoContent();
        }
    }
}
