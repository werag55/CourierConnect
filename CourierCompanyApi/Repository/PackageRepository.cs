using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class PackageRepository : Repository<Package>, IPackageRepository
    {
        private readonly ApplicationDbContext _db;
        public PackageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Package> UpdateAsync(Package entity)
        {
            _db.Packages.Update(entity);
            //await _db.SaveChangesAsync();
            return entity;
        }
    }
}
