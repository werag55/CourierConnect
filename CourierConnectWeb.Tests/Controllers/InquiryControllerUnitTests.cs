using AutoMapper;
using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnectWeb.Controllers;
using CourierConnectWeb.Services.Factory;
using CourierConnectWeb.Services.IServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnectWeb.Tests.Controllers
{
    public class InquiryControllerUnitTests
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<IdentityUser> _userManager;
        private IMapper _mapper;
        private List<IServiceFactory> _serviceFactories;

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
                    new CourierConnect.Models.Inquiry()
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
                            Id= i + 1,
                            dimensionsUnit = DimensionUnit.Inches,
                            weightUnit = WeightUnit.Kilograms,
                        },
                    }
                    ) ;
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        public InquiryControllerUnitTests() 
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userManager = A.Fake<UserManager<IdentityUser>>();
            _mapper = A.Fake<IMapper>();
            _serviceFactories = new List<IServiceFactory>();
        }

        [Fact]
        public async void InquiryController_Index_ReturnsSuccess()
        {
            //OurServiceFactory ourServiceFactory = A.Fake<OurServiceFactory>();
            //CurrierServiceFactory currierServiceFactory = A.Fake<CurrierServiceFactory>();
            //CourierHubServiceFactory courierHubServiceFactory = A.Fake<CourierHubServiceFactory>();


            //var dbContext = await GetDbContext();
            //dbContext.Inquiries.AsNoTracking();
            //var inquiryController = new InquiryController(_unitOfWork, ourServiceFactory,
            //    currierServiceFactory, courierHubServiceFactory, _mapper, _userManager, dbContext);


            ////var objInquiryList = A.Fake<IEnumerable<Inquiry>>();
            ////A.CallTo(() => _unitOfWork.Inquiry.GetAll(
            ////    A<string?>.Ignored
            ////    )).Returns(objInquiryList.ToList());

            //var fakeServiceFactory = A.Fake<IServiceFactory>();
            //var fakeInquiryService = A.Fake<IInquiryService>();
            //A.CallTo(() => fakeServiceFactory.createInquiryService())
            //    .Returns(fakeInquiryService);  //

            //A.CallTo(() => fakeInquiryService.GetAllAsync<APIResponse>())
            //    .Returns(A.Fake<APIResponse>());  //


            //var result = inquiryController.IndexAll(GetRandomSortOrder());

            //result.Should().BeOfType<Task<IActionResult>>();

            //var modelList = result.As<ObjectResult>().Value as List<Inquiry>;

            //modelList.Should().NotBeNull();  // Ensure the list is not null

            //foreach (var item in modelList)
            //{
            //    item.Should().BeOfType<Inquiry>();  // Assert the type of each item in the list
            //}

            ////.Which.Model.Should().BeEquivalentTo(objInquiryList);
        }


        private string GetRandomSortOrder()
        {
            Random random = new Random();
            return random.Next(2) == 0 ? "asc" : "desc";
        }
    }
}
