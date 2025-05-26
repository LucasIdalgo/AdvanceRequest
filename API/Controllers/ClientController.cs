using API.Models;
using API.Models.DTO;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public ClientController(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        [HttpGet("{Id}", Name = "GetClient")]
        public ActionResult GetClient(int Id)
        {
            var client = _clientRepository.GetClient(Id);
            if (client == null) return NotFound();

            ClientDTO clientDTO = _mapper.Map<Client, ClientDTO>(client);
            return Ok(clientDTO);
        }

        [HttpPost(Name = "PostClient")]
        public ActionResult PostClient([FromBody] ClientDTO client)
        {
            _clientRepository.PostClient(client);

            return CreatedAtRoute(routeName: "GetClient", routeValues: new { Id = client.ClientId }, value: client);
        }

        [HttpPut("{Id}", Name = "PutClient")]
        public ActionResult PutClient(int Id, [FromBody] ClientDTO client)
        {
            var obj = _clientRepository.GetClient(Id);
            if (obj == null) return NotFound();

            client.ClientId = Id;
            _clientRepository.PutClient(client);

            return Ok();
        }
    }
}
