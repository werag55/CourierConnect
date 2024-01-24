using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnectWeb.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private Mock<ApplicationDbContext> _context;
        private UserManager<IdentityUser> _userManager; // static so we cant test it!

        private InquiryController _inquiryController;
        public InquiryControllerUnitTests() 
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _context = new Mock<ApplicationDbContext>();
            _userManager = A.Fake<UserManager<IdentityUser>>();

            //SUT
            _inquiryController = new InquiryController(_unitOfWork, _userManager, CreateMockedDbContext());
        }

        private ApplicationDbContext CreateMockedDbContext()
        {
            var objInquiryList = A.Fake<List<Inquiry>>();
            var mockDbSet = new Mock<DbSet<Inquiry>>();
            mockDbSet.As<IQueryable<Inquiry>>().Setup(m => m.Provider).Returns(objInquiryList.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Inquiry>>().Setup(m => m.Expression).Returns(objInquiryList.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Inquiry>>().Setup(m => m.ElementType).Returns(objInquiryList.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Inquiry>>().Setup(m => m.GetEnumerator()).Returns(() => objInquiryList.GetEnumerator());
            // Set up the context mock to return the mock DbSet when Inquiries property is accessed
            _context.Setup(c => c.Inquiries).Returns(new Mock<DbSet<Inquiry>>().Object);

            // Return the mock context
            return _context.Object;
        }

        [Fact]
        public void InquiryController_Index_ReturnsSuccess()
        {
            var objInquiryList = A.Fake<List<Inquiry>>();
            A.CallTo(() => _unitOfWork.Inquiry.GetAll(A<string?>.Ignored)).Returns(objInquiryList.ToList());

            var result = _inquiryController.Index();

            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
