using CourierCompanyApi.Data;
using CourierCompanyApi.Models;
using CourierCompanyApi.Repository.IRepository;

namespace CourierCompanyApi.Repository
{
    public class PersonalDataRepository : Repository<PersonalData>, IPersonalDataRepository
    {
        private readonly ApplicationDbContext _db;
        public PersonalDataRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<PersonalData> UpdateAsync(PersonalData entity)
        {
            _db.PersonalData.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
