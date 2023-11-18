﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IInquiryRepository Inquiry { get; }

        void Save();
    }
}