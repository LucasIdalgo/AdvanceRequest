using API.DataBase;
using API.Models.DTO;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DesafioContext _db;
        private readonly IMapper _mapper;
        public ClientRepository(DesafioContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Client GetClient(int Id)
        {
            return _db.Client.AsNoTracking().FirstOrDefault(c => c.ClientId == Id);
        }

        public void PostClient(ClientDTO Client)
        {
            _db.Client.Add(_mapper.Map<ClientDTO, Client>(Client));
            _db.SaveChanges();
        }

        public void PutClient(ClientDTO Client)
        {
            _db.Client.Update(_mapper.Map<ClientDTO, Client>(Client));
            _db.SaveChanges();
        }
    }
}
