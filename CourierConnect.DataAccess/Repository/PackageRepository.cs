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
    public class PackageRepository : Repository<Package>, IPackageRepository
    {
        private ApplicationDbContext _db;
        public PackageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Package obj)
        {
            _db.Packages.Update(obj);
        }
    }
}
