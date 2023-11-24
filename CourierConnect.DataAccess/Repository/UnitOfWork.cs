using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IInquiryRepository Inquiry {  get; private set; }
        public IAddressRepository Address { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Inquiry = new InquiryRepository(_db);
            Address = new AddressRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
