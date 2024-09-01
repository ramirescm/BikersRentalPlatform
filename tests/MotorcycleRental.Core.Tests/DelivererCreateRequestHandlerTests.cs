using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Motorcycle.Core.Entities;
using Motorcycle.Core.Handlers;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.UoW;
using Motorcycle.Shared.Exceptions;
using Motorcycle.Shared.Requests;
using Motorcycle.Shared.Responses;
using NSubstitute;
using OperationResult;
using Xunit;

namespace Motorcycle.Core.Tests.Handlers
{
    public class DelivererCreateRequestHandlerTests
    {
        private readonly IDelivererRepository _delivererRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _uow;
        private readonly DelivererCreateRequestHandler _handler;

        public DelivererCreateRequestHandlerTests()
        {
            _delivererRepository = Substitute.For<IDelivererRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _uow = Substitute.For<IUnitOfWork>();
            _handler = new DelivererCreateRequestHandler(_delivererRepository, _uow, _userRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenCnpjAlreadyExists()
        {
            // Arrange
            var request = new DelivererCreateRequest(
                Name: "John Doe",
                Cnpj: "12345678901234",
                BirthDate: "1990-01-01",
                CnhNumber: "12345678901",
                CnhType: "A",
                CnhPathImage: "/path/to/image",
                Email: "john.doe@example.com"
            );
            
            _delivererRepository.CnpjExistsAsync(request.Cnpj, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<BadRequestException>()
                .Which.Message.Should().Be("CNPJ already exists");
            await _delivererRepository.Received().CnpjExistsAsync(request.Cnpj, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenCnhAlreadyExists()
        {
            // Arrange
            var request = new DelivererCreateRequest(
                Name: "John Doe",
                Cnpj: "12345678901234",
                BirthDate: "1990-01-01",
                CnhNumber: "12345678901",
                CnhType: "A",
                CnhPathImage: "/path/to/image",
                Email: "john.doe@example.com"
            );

            _delivererRepository.CnpjExistsAsync(request.Cnpj, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(false));
            _delivererRepository.CnhExistsAsync(request.CnhNumber, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(true));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType<BadRequestException>()
                .Which.Message.Should().Be("CNH already exists");
            await _delivererRepository.Received().CnhExistsAsync(request.CnhNumber, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenDelivererIsCreated()
        {
            // Arrange
            var request = new DelivererCreateRequest(
                Name: "John Doe",
                Cnpj: "12345678901234",
                BirthDate: "1990-01-01",
                CnhNumber: "12345678901",
                CnhType: "A",
                CnhPathImage: "/path/to/image",
                Email: "john.doe@example.com"
            );

            _delivererRepository.CnpjExistsAsync(request.Cnpj, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(false));
            _delivererRepository.CnhExistsAsync(request.CnhNumber, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(false));

            var delivererId = Guid.NewGuid();
            _delivererRepository.Add(Arg.Any<Deliverer>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(delivererId));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(delivererId.ToString());
            await _delivererRepository.Received().Add(Arg.Any<Deliverer>(), Arg.Any<CancellationToken>());
            await _userRepository.Received().Create(Arg.Any<User>(), Arg.Any<CancellationToken>());
            await _uow.Received().CommitAsync(Arg.Any<CancellationToken>());
        }
    }
}
