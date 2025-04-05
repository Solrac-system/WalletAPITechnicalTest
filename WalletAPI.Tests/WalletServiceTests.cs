using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain;
using WalletAPI.Infrastructure.Services;

namespace WalletAPI.Tests
{
    public class WalletServiceTests
    {
        private readonly Mock<IWalletRespository> _walletRepositoryMock;
        private readonly WalletService _walletService;
        private readonly ILogger<WalletServiceTests> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public WalletServiceTests()
        {
            _walletRepositoryMock = new Mock<IWalletRespository>();

            // Configura el servicio usando el Mock del repositorio
            _walletService = new WalletService(_walletRepositoryMock.Object, Mock.Of<IMapper>(), Mock.Of<ILogger<WalletService>>());
        }

        [Fact]
        public async Task CreateWalletAsync_ShouldReturnWallet_WhenValidData()
        {
            // Arrange
            var walletDto = new WalletWriteDto
            {
                DocumentId = "123456789",
                Name = "Test Wallet",
                Balance = 1000.00m
            };

            var wallet = new Wallet
            {
                Id = 1,
                DocumentId = walletDto.DocumentId,
                Name = walletDto.Name,
                Balance = walletDto.Balance,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Mock del mapper
            _mapperMock.Setup(mapper => mapper.Map<Wallet>(walletDto))
                .Returns(wallet);

            // Mock del repositorio
            _walletRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Wallet>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _walletService.CreateWalletAsync(walletDto);

            // Assert
            result.Should().NotBeNull(); // La billetera no debe ser null
            result.DocumentId.Should().Be(walletDto.DocumentId); // Verifica el DocumentId
            result.Name.Should().Be(walletDto.Name); // Verifica el nombre
            result.Balance.Should().Be(walletDto.Balance); // Verifica el balance
            _walletRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Wallet>()), Times.Once); // Verifica que el repositorio fue llamado
        }


    }
}
