using CourierCompanyApi.Models;

namespace CourierCompanyApi.Repository.IRepository
{
    public interface IPersonalDataRepository : IRepository<PersonalData>
    {
        Task<PersonalData> UpdateAsync(PersonalData entity);
    }
}
