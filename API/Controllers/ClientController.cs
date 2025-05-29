using API.Models;
using API.Models.DTO;
using API.Models.Responses;
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
            if (client == null)
                return NotFound(new ResponseDefault<ClientDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Usuário não encontrado"
                });

            ClientDTO clientDTO = _mapper.Map<Client, ClientDTO>(client);
            return Ok(new ResponseDefault<ClientDTO>
            {
                Status = Ok().StatusCode.ToString(),
                Message = "Sucesso",
                Data = clientDTO
            });
        }

        [AllowAnonymous]
        [HttpGet("login", Name = "ClientLogin")]
        public ActionResult ClientLogin([FromQuery] ClientLoginDTO login)
        {
            var client = _clientRepository.GetClientByEmail(login.Email);
            if (client == null)
                return Unauthorized(new ResponseDefault<ClientDTO>
                {
                    Status = Unauthorized().StatusCode.ToString(),
                    Message = "Usuário não cadastrado"
                });

            if (!_clientRepository.Login(_mapper.Map<Client, ClientDTO>(client), login))
                return Unauthorized(new ResponseDefault<ClientDTO>
                {
                    Status = Unauthorized().StatusCode.ToString(),
                    Message = "Email ou senha inválidos"
                });

            var Token = _token.GenerateToken(client.Email);

            return Ok(new ResponseDefault<ClientTokenDTO>
            {
                Data = new ClientTokenDTO { ClientId = client.ClientId, Name = client.Name, Token = Token },
                Status = Ok().StatusCode.ToString(),
                Message = "Token gerado"
            });
        }

        [AllowAnonymous]
        [HttpPost(Name = "PostClient")]
        public ActionResult PostClient([FromBody] ClientDTO client)
        {
            if (_clientRepository.GetClientByEmail(client.Email) != null)
                return BadRequest(new ResponseDefault<ClientDTO>
                {
                    Status = BadRequest().StatusCode.ToString(),
                    Message = "Usuário já existe"
                });

            _clientRepository.PostClient(client);

            return CreatedAtRoute(routeName: "GetClient", routeValues: new { Id = client.ClientId }, value: client);
        }

        
        [HttpPut("{Id}", Name = "PutClient")]
        public ActionResult PutClient(int Id, [FromBody] ClientDTO client)
        {
            var obj = _clientRepository.GetClient(Id);
            if (obj == null)
                return NotFound(new ResponseDefault<ClientDTO>
                {
                    Status = NotFound().StatusCode.ToString(),
                    Message = "Usuário não encontrado"
                });

            client.ClientId = Id;
            _clientRepository.PutClient(client);

            return Ok(new ResponseDefault<ClientDTO>
            {
                Data = client,
                Status = Ok().StatusCode.ToString(),
                Message = "Usuário alterado com sucesso"
            });
        }
    }
}
