using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IInquiryRepository : IRepository<Inquiry>
    {
        Task<Inquiry> UpdateAsync(Inquiry entity);
    }
}
