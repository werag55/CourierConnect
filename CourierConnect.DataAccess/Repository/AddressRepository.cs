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
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private ApplicationDbContext _db;
        public AddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Address obj)
        {
            _db.Addresses.Update(obj);
        }
    }
}
