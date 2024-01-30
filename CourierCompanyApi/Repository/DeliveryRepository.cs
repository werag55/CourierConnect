using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
    {
        private readonly ApplicationDbContext _db;
        public DeliveryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Delivery> UpdateAsync(Delivery entity)
        {
            _db.Deliveries.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
