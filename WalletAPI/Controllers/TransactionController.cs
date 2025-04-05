using Microsoft.AspNetCore.Mvc;
using WalletAPI.Application.Contracts.Services;
using WalletAPI.Application.DTOs;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;
        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TransactionWriteDto transactionDto)
        {
            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(transactionDto);
                return Ok(transaction);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Transaction failed: Wallet not found.");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid transaction data: {Error}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Transaction failed: {Error}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating transaction.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetByWalletId(int walletId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsAsync(walletId);
                return Ok(transactions);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for WalletId {WalletId}", walletId);
                return StatusCode(500, ex.Message);
            }
        }




    }
}