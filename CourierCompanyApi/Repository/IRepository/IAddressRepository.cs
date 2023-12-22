using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> UpdateAsync(Address entity);
    }
}
