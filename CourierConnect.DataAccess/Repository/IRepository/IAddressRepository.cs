using CourierConnect.Models;

namespace CourierConnect.DataAccess.Repository.IRepository
{
    public interface IAddressRepository : IRepository<Address>
    {
        void Update(Address obj);
    }
}
