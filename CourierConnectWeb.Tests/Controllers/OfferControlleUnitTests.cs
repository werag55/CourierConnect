using AutoMapper;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnectWeb.Controllers;
using CourierConnectWeb.Services.IServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnectWeb.Tests.Controllers
{
    public class OfferControlleUnitTests
    {
        private readonly OfferController _offerController;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;

        public OfferControlleUnitTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _offerService = A.Fake<IOfferService>();
            _userManager = A.Fake<UserManager<IdentityUser>>();
            _mapper = A.Fake<IMapper>();

            //SUT
            _offerController = new OfferController(_unitOfWork, _offerService, _mapper, _userManager);
        }

        [Fact]
        public void OfferController_Index_ReturnsSuccess()
        {
            int id = 1;
            var offer = new Offer { Id = id};
            var objOfferList = A.Fake<List<Offer>>();

            A.CallTo(() => _unitOfWork.Offer.FindAll(
            A<Expression<Func<Offer, bool>>>.Ignored,
            A<string>.Ignored
            )).Returns(objOfferList);

            var result = _offerController.Index(id);

            result.Should().BeOfType<ViewResult>().Which.Model.As<IEnumerable<Offer>>().Should().BeEquivalentTo(objOfferList);
        }

        [Fact]
        public void OfferController_Index_ReturnsNotFoundWhenNoOffers()
        {
            int inquiryId = 1;
            A.CallTo(() => _unitOfWork.Offer.FindAll(
                A<Expression<Func<Offer, bool>>>.Ignored,
                A<string>.Ignored
            )).Returns<IEnumerable<Offer>>(null);

            var result = _offerController.Index(inquiryId);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
