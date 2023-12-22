using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class CourierRepository : Repository<Courier>, ICourierRepository
    {
        private readonly ApplicationDbContext _db;
        public CourierRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Courier> UpdateAsync(Courier entity)
        {
            _db.Couriers.Update(entity);
            //await _db.SaveChangesAsync();
            return entity;
        }
    }
}
