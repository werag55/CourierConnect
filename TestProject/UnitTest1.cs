using CourierConnect.DataAccess.Repository;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnectWeb.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid.Helpers.Mail;
using System;
using System.Security.Principal;
using System.Web.Mvc;
namespace TestProject
{
    [TestClass]
    public class InquiryControllerUnitTest
    {
        Inquiry i1;
        Inquiry i2;
        Inquiry i3;
        List<Inquiry> inquiries;
        InquiryController iController;
        Mock<IUnitOfWork> unitOfWork;
        public InquiryControllerUnitTest()
        {
            i1 = new Inquiry { Id = 1, clientId = "client1", isPriority = true };
            i2 = new Inquiry { Id = 2, clientId = "client1", isPriority = false };
            i3 = new Inquiry { Id = 2, clientId = "client2", isPriority = false };
            inquiries = new List<Inquiry> { i1, i2, i3 };
            unitOfWork = new Mock<IUnitOfWork>();
            //var userManager = new UserManager<IdentityUser>();
            //UserManager<IdentityUser> userManager = GetUserManager();
            //var mockuser = new Mock<UserManager<IdentityUser>>();
            //var context = new Mock<CourierConnect.DataAccess.Data.ApplicationDbContext>();
            
        }
        [TestMethod]
        public void Index()
        {
            // unit of work setup
            var mockuser = new Mock<UserManager<IdentityUser>>();
            var context = new Mock<CourierConnect.DataAccess.Data.ApplicationDbContext>();
            unitOfWork.Setup(u => u.Inquiry).Returns((IInquiryRepository)(inquiries.ToList() ));
            iController = new InquiryController(unitOfWork.Object, mockuser.Object, context.Object);
            // action
            var result = iController.Index() as ViewResult;
            var model = result.Model as List<Inquiry>;

            // assert
            CollectionAssert.Contains(model, i1);
            CollectionAssert.Contains(model, i2);
            //var inquiry = new InquiryController(mockuser.Object);
        }
    }
}