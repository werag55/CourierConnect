using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IOfferRepository : IRepository<Offer>
    {
        Task<Offer> UpdateAsync(Offer entity);
    }
}
