using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<Request> UpdateAsync(Request entity);
    }
}
