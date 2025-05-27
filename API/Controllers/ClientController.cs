using API.Models;
using API.Models.DTO;
using API.Repositories.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly TokenService _token;
        private readonly IMapper _mapper;
        public ClientController(IClientRepository clientRepository, IMapper mapper, TokenService token)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            _token = token;
        }

        [HttpGet("{Id}", Name = "GetClient")]
        public ActionResult GetClient(int Id)
        {
            var client = _clientRepository.GetClient(Id);
            if (client == null) return NotFound();

            ClientDTO clientDTO = _mapper.Map<Client, ClientDTO>(client);
            return Ok(clientDTO);
        }

        [AllowAnonymous]
        [HttpGet("login", Name = "ClientLogin")]
        public ActionResult ClientLogin(ClientLoginDTO login)
        {
            var client = _clientRepository.GetClientByEmail(login.Email);
            if (client == null) return Unauthorized("Email ou senha inválidos");

            if (!_clientRepository.Login(_mapper.Map<Client, ClientDTO>(client), login))
                return Unauthorized("Email ou senha inválidos");

            var Token = _token.GenerateToken(client.Email);

            return Ok(Token);
        }

        [AllowAnonymous]
        [HttpPost(Name = "PostClient")]
        public ActionResult PostClient([FromBody] ClientDTO client)
        {
            if (_clientRepository.GetClientByEmail(client.Email) != null)
                return BadRequest("Usuário já existe");

            _clientRepository.PostClient(client);

            return CreatedAtRoute(routeName: "GetClient", routeValues: new { Id = client.ClientId }, value: client);
        }

        [Authorize]
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
