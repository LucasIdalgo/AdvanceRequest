using API.DataBase;
using API.Models.DTO;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DesafioContext _db;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<ClientDTO> _hasher = new();
        public ClientRepository(DesafioContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Client GetClient(int Id)
        {
            return _db.Client.AsNoTracking().FirstOrDefault(c => c.ClientId == Id);
        }

        public Client GetClientByEmail(string Email)
        {
            return _db.Client.AsNoTracking().FirstOrDefault(c => c.Email == Email);
        }

        public bool Login(ClientDTO Client, ClientLoginDTO login)
        {
            var result = _hasher.VerifyHashedPassword(Client, Client.Password, login.Password);
            if (result == PasswordVerificationResult.Failed) return false;

            return true;
        }

        public void PostClient(ClientDTO Client)
        {
            Client.Password = _hasher.HashPassword(Client, Client.Password);
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
