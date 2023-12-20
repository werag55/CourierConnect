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
    public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
    {
        private ApplicationDbContext _db;
        public DeliveryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Delivery obj)
        {
            _db.Deliveries.Update(obj);
        }
    }
}
