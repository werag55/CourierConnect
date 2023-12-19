using CourierConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.DataAccess.Repository.IRepository
{
    public interface IPersonalDataRepository : IRepository<PersonalData>
    {
        void Update(PersonalData obj);
    }
}
