using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IPackageRepository : IRepository<Package>
    {
        Task<Package> UpdateAsync(Package entity);
    }
}
