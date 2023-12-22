using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class InquiryRepository : Repository<Inquiry>, IInquiryRepository
    {
        private readonly ApplicationDbContext _db;
        public InquiryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Inquiry> UpdateAsync(Inquiry entity)
        {
            _db.Inquiries.Update(entity);
            //await _db.SaveChangesAsync();
            return entity;
        }
    }
}
