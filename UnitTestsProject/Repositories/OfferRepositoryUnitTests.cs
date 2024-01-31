using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository;
using CourierConnect.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnectWeb.Tests.Repositories
{
    public class OfferRepositoryUnitTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Offers.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Offers.Add(
                    new CourierConnect.Models.Offer()
                    {
                        Id = i+1,
                        companyOfferId = i.ToString()+1,
                        companyId = i+1,
                        inquiryId = i+1,
                        inquiry = new CourierConnect.Models.Inquiry() 
                        {
                            Id = i+1,

                        },
                        creationDate = DateTime.Now,
                        updatedDate = DateTime.Now,
                        expirationDate = DateTime.Now,
                        status = CourierConnect.Models.OfferStatus.Pending,
                        price = 20,
                        taxes = 5,
                        fees = 3,
                        currency = CourierConnect.Models.Currency.PLN
                    }
                    );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void OfferRepository_Update()
        {
            var updatedOffer = new CourierConnect.Models.Offer()
            {
                Id = 1,
                companyOfferId = "1",
                companyId = 1,
                inquiryId = 1,
                inquiry = new CourierConnect.Models.Inquiry() { },
                creationDate = DateTime.Now,
                updatedDate = DateTime.Now,
                expirationDate = DateTime.Now,
                status = CourierConnect.Models.OfferStatus.Pending,
                price = 20,
                taxes = 5,
                fees = 3,
                currency = CourierConnect.Models.Currency.PLN
            };

            var dbContext = await GetDbContext();
            dbContext.Offers.AsNoTracking();
            var repository = new Repository<Offer>(dbContext);

            var result = repository.Get(u => u.Id == updatedOffer.Id);

            result.Should().NotBeNull();
            result.Should().BeOfType<Offer>();
            result.Id.Should().Be(updatedOffer.Id);
        }
    }
}
