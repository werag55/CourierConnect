using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.DataAccess.Repository
{
    public class PersonalDataRepository : Repository<PersonalData>, IPersonalDataRepository
    {
        private ApplicationDbContext _db;
        public PersonalDataRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PersonalData obj)
        {
            _db.PersonalData.Update(obj);
        }
    }
}
