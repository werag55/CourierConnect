using AutoMapper;
using CourierCompanyApi.Controllers;
using CourierCompanyApi.Models;
using CourierCompanyApi.Models.Dto;
using CourierCompanyApi.Repository;
using CourierCompanyApi.Repository.IRepository;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnectWeb.Tests.CourierCompanyApi.Controllers
{
    public class DeliveryControllerUnitTests
    {
        private readonly APIResponse _responde;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DeliveryController _controller;
        public DeliveryControllerUnitTests()
        {
            _responde = new ();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _mapper = A.Fake<IMapper>();
            _controller = new DeliveryController(_unitOfWork, _mapper);
        }

        [Fact]
        public async void DeliveryController_GetDelivery_ReturnsOK()
        {
            string deliveryId = "1";
            var delivery = new Delivery()
            {
                Id = 1,
                GUID = "1",
                courierId = 1,
                courier = new Courier()
                {
                    Id = 1,
                },
                requestId = 1,
                request = new Request()
                {
                    Id =1,
                },
                cancelationDeadline = DateTime.Now,
                deliveryStatus = DeliveryStatus.Proccessing,
            };
            var deliveryDto = new DeliveryDto()
            {
                companyDeliveryId = "1",
                courier = new CourierDto()
                {
                    name = "John",
                    surname = "Pedro"
                },
                request = new RequestDto()
                {
                    requestStatus = RequestStatus.Pending
                },
                cancelationDeadline = DateTime.Now,
                deliveryStatus = DeliveryStatus.Proccessing,
            };

            A.CallTo(() => _unitOfWork.Delivery.GetAsync(
            A<Expression<Func<Delivery, bool>>>.Ignored,
            A<bool>.Ignored,
            A<string>.Ignored
            )).Returns(delivery);
            A.CallTo(() => _mapper.Map<DeliveryDto>(delivery)).Returns(deliveryDto);

            var result = await _controller.GetDelivery(deliveryId);

            result.Should().BeOfType<ActionResult<APIResponse>>();
            var okObjectResult = (OkObjectResult)result.Result;
            var apiResponse = (APIResponse)okObjectResult.Value;

            apiResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.ErrorMessages.Should().BeEmpty();
            apiResponse.Result.Should().BeEquivalentTo(deliveryDto);
        }

        [Fact]
        public async Task GetDelivery_ReturnsBadRequestWhenDeliveryNotFound()
        {
            string deliveryId = "1";

            A.CallTo(() => _unitOfWork.Delivery.GetAsync(
                A<Expression<Func<Delivery, bool>>>.Ignored,
                A<bool>.Ignored,
                A<string>.Ignored
            )).Returns<Delivery>(null);

            var result = await _controller.GetDelivery(deliveryId);

            result.Should().BeOfType<ActionResult<APIResponse>>();
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task GetDelivery_ReturnsBadRequestOnException()
        {
            string deliveryId = "1";

            A.CallTo(() => _unitOfWork.Delivery.GetAsync(
                A<Expression<Func<Delivery, bool>>>.Ignored,
                A<bool>.Ignored,
                A<string>.Ignored
            )).Throws<Exception>();

            var result = await _controller.GetDelivery(deliveryId);

            result.Should().BeOfType<ActionResult<APIResponse>>();
            var response = result.Result as BadRequestObjectResult;
            response?.StatusCode.Should().Be(400);
            response?.Value.Should().NotBeNull();
            var API = response.Value as APIResponse;
            API?.IsSuccess.Should().BeFalse();
            API?.ErrorMessages.Should().NotBeEmpty();
        }
    }
}
