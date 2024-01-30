using CourierCompanyApi.Data;
using Microsoft.EntityFrameworkCore;
using CourierCompanyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CourierCompanyApi.Repository;
using FakeItEasy;
using FluentAssertions;

namespace CourierConnectWeb.Tests.UnitTests.CourierCompanyApi.Repositories
{
    public class RepositoryUnitTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Offers.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Offers.Add(
                    new Offer()
                    {
                        Id = i + 1,
                        inquiryId = i + 1,
                        inquiry = new Inquiry()
                        {
                            Id = i + 1,

                        },
                        creationDate = DateTime.Now,
                        updatedDate = DateTime.Now,
                        expirationDate = DateTime.Now,
                        status = OfferStatus.Pending,
                        price = 20,
                        taxes = 5,
                        fees = 3,
                        currency = Currency.PLN
                    }
                    );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void Repository_GetAllAsync_ReturnsListOfOffers()
        {
            var dbContext = await GetDbContext();
            dbContext.Offers.AsNoTracking();
            var _repository = new Repository<Offer>(dbContext);

            var result = _repository.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeOfType<Task<List<Offer>>>();
        }
    }
}
