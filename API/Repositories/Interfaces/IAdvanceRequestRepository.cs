using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Repositories.Interfaces
{
    public interface IAdvanceRequestRepository
    {
        public ResponseAll<AdvanceRequestDTO> GetAllAdvanceRequest(UrlQuery query);
        public AdvanceRequest GetAdvanceRequest(int Id);
        public void PostAdvanceRequest(AdvanceRequestDTO AdvanceRequest);
        public void PutAdvanceRequest(List<AdvanceRequestDTO> AdvanceRequest);
        public void DeleteAdvanceRequest(int Id);
    }
}
