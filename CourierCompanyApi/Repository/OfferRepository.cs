using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class OfferRepository : Repository<Offer>, IOfferRepository
    {
        private readonly ApplicationDbContext _db;
        public OfferRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Offer> UpdateAsync(Offer entity)
        {
            _db.Offers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
