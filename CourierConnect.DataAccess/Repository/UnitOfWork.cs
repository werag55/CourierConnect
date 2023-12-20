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
        public IPackageRepository Package { get; private set; }
        public IPersonalDataRepository PersonalData { get; private set; }
        public IOfferRepository Offer { get; private set; }
        public IRequestRepository Request { get; private set; }
        public IDeliveryRepository Delivery { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Inquiry = new InquiryRepository(_db);
            Address = new AddressRepository(_db);
            Package = new PackageRepository(_db);
            PersonalData = new PersonalDataRepository(_db);
            Offer = new OfferRepository(_db);
            Request = new RequestRepository(_db);
            Delivery = new DeliveryRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
