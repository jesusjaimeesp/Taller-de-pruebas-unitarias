﻿using AutoMapper;
using Moq;
using Ordering.Application.Contracts;
using Ordering.Application.Features.Commands.Checkout;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Test.Features
{
    public class CheckoutOrderCommandHandlerTest
    {
        //[Fact]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handle_Should_Call_AddAsync_And_Return_Id(int id)
        {
            //Arrange
            Mock<IGenericRepository<Order>> _repositoryMock = new();
            Mock<IMapper> _mapperMock = new();
            var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(_repositoryMock.Object, _mapperMock.Object);
            var checkoutOrderCommandTest = new CheckoutOrderCommand()
            {
                Address = "you400@gamail.com",
                FirstName = "Yael",
                LastName = "Chavez",
                PaymentMethod = 1,
                TotalPrice = 10,
                UserName = "YaelChavez"
            };

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(new Order { Id = id });

            _mapperMock.Setup(mapper => mapper.Map<Order>(checkoutOrderCommandTest))
                .Returns(new Order { Id = id });

            //Act
            var result = await checkoutOrderCommandHandler.Handle(checkoutOrderCommandTest, CancellationToken.None);

            //Assert
            Assert.Equal(id, result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once);

        }
    }
}
