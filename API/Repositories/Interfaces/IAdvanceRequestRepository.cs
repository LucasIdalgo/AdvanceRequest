using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IAdvanceRequestRepository
    {
        public List<AdvanceRequest> GetAllAdvanceRequest();
        public AdvanceRequest GetAdvanceRequest(int Id);
        public void PostAdvanceRequest(AdvanceRequest AdvanceRequest);
        public void PutAdvanceRequest(AdvanceRequest AdvanceRequest);
        public void DeleteAdvanceRequest(int Id);
    }
}
