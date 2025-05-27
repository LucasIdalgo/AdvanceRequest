using API.Models.DTO;
using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Client GetClient(int Id);
        public Client GetClientByEmail(string Email);
        public void PostClient(ClientDTO Client);
        public void PutClient(ClientDTO Client);

        public bool Login(ClientDTO Client, ClientLoginDTO login);
    }
}
