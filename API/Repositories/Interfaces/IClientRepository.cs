using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Client GetClient(int Id);
        public void PostClient(Client Client);
        public void PutClient(Client Client);
        public void DeleteClient(int Id);
    }
}
