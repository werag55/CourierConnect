using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private readonly ApplicationDbContext _db;
        public RequestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Request> UpdateAsync(Request entity)
        {
            _db.Requests.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
