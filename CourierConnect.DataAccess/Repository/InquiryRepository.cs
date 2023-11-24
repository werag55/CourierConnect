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
    public class InquiryRepository : Repository<Inquiry>, IInquiryRepository
    {
        private ApplicationDbContext _db;
        public InquiryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Inquiry obj)
        {
            _db.Inquiries.Update(obj);
        }
    }
}
