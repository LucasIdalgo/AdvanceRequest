using API.DataBase;
using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AdvanceRequestRepository : IAdvanceRequestRepository
    {
        private readonly DesafioContext _db;
        private readonly IMapper _mapper;
        public AdvanceRequestRepository(DesafioContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public ResponseAll<AdvanceRequestDTO> GetAllAdvanceRequest(UrlQuery query)
        {
            var retorno = new ResponseAll<AdvanceRequestDTO>();
            var item = _db.AdvanceRequest.AsNoTracking().AsQueryable();

            var pagination = new Pagination
            {
                Page = query.Page,
                LimitByPages = query.Limit,
                Total = item.Count(),
                TotalPages = (int)Math.Ceiling((double)item.Count() / query.Limit)
            };

            item = item.Skip((query.Page - 1) * query.Limit).Take(query.Limit);

            retorno.Pagination = pagination;
            retorno.Data = _mapper.Map<List<AdvanceRequest>, List<AdvanceRequestDTO>>(item.ToList());

            return retorno;
        }

        public AdvanceRequest GetAdvanceRequest(int Id)
        {
            return _db.AdvanceRequest.AsNoTracking().FirstOrDefault(a => a.AdvanceRequestId == Id);
        }

        public void PostAdvanceRequest(AdvanceRequestDTO AdvanceRequest)
        {
            _db.AdvanceRequest.Add(_mapper.Map<AdvanceRequestDTO, AdvanceRequest>(AdvanceRequest));
            _db.SaveChanges();
        }

        public void PutAdvanceRequest(List<AdvanceRequestDTO> AdvanceRequest)
        {

            foreach (var item in AdvanceRequest)
            {
                item.Approve = true;
                item.ApprovedAt = DateTime.Now;

                _db.AdvanceRequest.Update(_mapper.Map<AdvanceRequestDTO, AdvanceRequest>(item));
            }

            _db.SaveChanges();
        }

        public void DeleteAdvanceRequest(int Id)
        {
            _db.AdvanceRequest.Remove(GetAdvanceRequest(Id));
        }
    }
}
