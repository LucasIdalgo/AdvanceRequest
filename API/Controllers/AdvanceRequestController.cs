using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AdvanceRequestController : ControllerBase
    {
        private readonly IAdvanceRequestRepository _advanceRequestRepository;
        private readonly IMapper _mapper;
        public AdvanceRequestController(IAdvanceRequestRepository advanceRequestRepository, IMapper mapper)
        {
            _advanceRequestRepository = advanceRequestRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetAllAdvanceRequests")]
        public ActionResult GetAllAdvanceRequests([FromQuery] UrlQuery query)
        {
            var item = _advanceRequestRepository.GetAllAdvanceRequest(query);

            if (item.Data.Count == 0 || query.Page > item.Pagination.TotalPages) return NotFound();

            return Ok(item);
        }

        [HttpGet("{Id}", Name = "GetAdvanceRequest")]
        public ActionResult GetAdvanceRequest(int Id)
        {
            var advanceRequest = _advanceRequestRepository.GetAdvanceRequest(Id);
            if (advanceRequest == null) return NotFound();

            AdvanceRequestDTO advanceRequestDTO = _mapper.Map<AdvanceRequest, AdvanceRequestDTO>(advanceRequest);
            return Ok(advanceRequestDTO);
        }

        [HttpPost(Name = "PostAdvanceRequest")]
        public ActionResult PostAdvanceRequest([FromBody] AdvanceRequestDTO advanceRequest)
        {
            _advanceRequestRepository.PostAdvanceRequest(advanceRequest);

            return CreatedAtRoute(routeName: "GetAdvanceRequest", routeValues: new { Id = advanceRequest.AdvanceRequestId }, value: advanceRequest);
        }

        [HttpPut("/approve", Name = "PutAdvanceRequest")]
        public ActionResult PutAdvanceRequest([FromBody] List<AdvanceRequestDTO> advanceRequest)
        {
            foreach (var item in advanceRequest)
            {
                var obj = _advanceRequestRepository.GetAdvanceRequest(item.AdvanceRequestId);
                if (obj == null) return NotFound();
            }

            _advanceRequestRepository.PutAdvanceRequest(advanceRequest);

            return Ok();
        }

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
