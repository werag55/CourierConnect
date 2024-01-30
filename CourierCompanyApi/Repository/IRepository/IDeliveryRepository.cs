using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<Delivery> UpdateAsync(Delivery entity);
    }
}
