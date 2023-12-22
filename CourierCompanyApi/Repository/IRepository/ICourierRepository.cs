using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface ICourierRepository : IRepository<Courier>
    {
        Task<Courier> UpdateAsync(Courier entity);
    }
}
