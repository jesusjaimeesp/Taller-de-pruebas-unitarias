using AutoMapper;
using Moq;
using Ordering.Application.Contracts;
using Ordering.Application.Features.Commands.Checkout;
using Ordering.Application.Features.Queries;
using Ordering.Domain.Entities;
using System.Linq.Expressions;

namespace Ordering.Test.Features
{
    public class GetOrdersListQueryHandlerTest
    {
        [Fact]
        public async Task Handle_Should_Call_GetAsync_And_ReturnList()
        {
            //Arrange
            Mock<IGenericRepository<Order>> _repositoryMock = new();
            Mock<IMapper> _mapperMock = new();

            var getOrdersListQueryHandler = new GetOrdersListQueryHandler(_repositoryMock.Object, _mapperMock.Object);
            var getOrderListQuery = new GetOrdersListQuery()
            {
                UserName = "Yael"
            };

            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Order,bool>>>()))
               .ReturnsAsync(new List<Order>());

            _mapperMock.Setup(mapper => mapper.Map<List<OrdersViewModel>>(It.IsAny<List<Order>>()))
                .Returns(new List<OrdersViewModel>());

            //Act
            var result = await getOrdersListQueryHandler.Handle(getOrderListQuery, CancellationToken.None);

            //Assert
            _repositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()),Times.Once);
            Assert.IsType<List<OrdersViewModel>>(result);
            _mapperMock.Verify(map => map.Map<List<OrdersViewModel>>(It.IsAny<List<Order>>()),Times.Once);

        }
    }
}
