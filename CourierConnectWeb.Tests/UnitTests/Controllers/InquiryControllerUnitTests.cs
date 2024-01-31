using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnectWeb.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnectWeb.Tests.UnitTests.Controllers
{
    public class InquiryControllerUnitTests
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<IdentityUser> _userManager;


        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Inquiries.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Inquiries.Add(
                    new Inquiry()
                    {
                        Id = i + 1,
                        creationDate = DateTime.Now,
                        pickupDate = DateTime.Now,
                        deliveryDate = DateTime.Now,
                        isCompany = false,
                        isPriority = false,
                        weekendDelivery = true,
                        sourceAddressId = i + 1,
                        sourceAddress = new Address()
                        {
                            Id = i + 1,
                            city = "Warszawa",
                            postcode = "00000",
                            streetName = "Wolska",

                        },
                        destinationAddressId = i + 1,
                        destinationAddress = new Address()
                        {
                            Id = i + 1,
                            city = "Warszawa",
                            postcode = "00001",
                            streetName = "Woloska",
                        },
                        packageId = i + 1,
                        package = new Package()
                        {
                            Id = i + 1,
                            dimensionsUnit = "m",
                            weightUnit = "kg",
                        },
                    }
                    );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        public InquiryControllerUnitTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userManager = A.Fake<UserManager<IdentityUser>>();
        }

        [Fact]
        public async void InquiryController_Index_ReturnsSuccess()
        {
            var dbContext = await GetDbContext();
            dbContext.Inquiries.AsNoTracking();
            var inquiryController = new InquiryController(_unitOfWork, _userManager, dbContext);


            var objInquiryList = A.Fake<IEnumerable<Inquiry>>();
            A.CallTo(() => _unitOfWork.Inquiry.GetAll(
                A<string?>.Ignored
                )).Returns(objInquiryList.ToList());

            var result = inquiryController.Index();

            result.Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeEquivalentTo(objInquiryList);
        }
    }
}
