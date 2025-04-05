using Microsoft.AspNetCore.Mvc;
using WalletAPI.Application.Contracts.Services;
using WalletAPI.Application.DTOs;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;
        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WalletWriteDto walletDto)
        {
            try
            {
                var wallet = await _walletService.CreateWalletAsync(walletDto);
                return Ok(wallet);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating wallet.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var wallet = await _walletService.GetWalletAsync(id);
                return Ok(wallet);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found: {WalletId}", id);
                return NotFound(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error retrieving wallet.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WalletWriteDto walletDto)
        {
            try
            {
                await _walletService.UpdateWalletAsync(id, walletDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found for update: {WalletId}", id);
                return NotFound(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating wallet.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _walletService.DeleteWalletAsync(id);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting wallet.");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
