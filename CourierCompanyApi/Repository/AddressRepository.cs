using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly ApplicationDbContext _db;
        public AddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Address> UpdateAsync(Address entity)
        {
            _db.Addresses.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
