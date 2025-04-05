using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;
using WalletAPI.Application.DTOs;

namespace WalletAPI.Tests
{
    public class WalletControllerTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public WalletControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetWallet_ShouldReturnWallet_WhenWalletExists()
        {
            // Act
            var response = await _client.GetAsync("/api/Wallet/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var wallet = JsonConvert.DeserializeObject<WalletReadDto>(await response.Content.ReadAsStringAsync());
            wallet.Should().NotBeNull();
        }


    }
}
