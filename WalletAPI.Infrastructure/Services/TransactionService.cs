using AutoMapper;
using Microsoft.Extensions.Logging;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Application.Contracts.Services;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain;

namespace WalletAPI.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRespository _transactionRepository;
        private readonly IWalletRespository _walletRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRespository transactionRepository, IWalletRespository walletRespository, IMapper mapper, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRespository;
            _mapper = mapper;
            _logger = logger;
        }


  
        public async Task<TransactionReadDto> CreateTransactionAsync(TransactionWriteDto transactionDto)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(transactionDto.WalletId)
                    ?? throw new KeyNotFoundException("Wallet not found.");

                if (transactionDto.Amount <= 0)
                    throw new ArgumentException("Transaction amount must be greater than zero.");

                if (transactionDto.Type == "Debit" && wallet.Balance < transactionDto.Amount)
                    throw new InvalidOperationException("Insufficient balance.");

                wallet.Balance += transactionDto.Type == "Credit" ? transactionDto.Amount : -transactionDto.Amount;
                wallet.UpdatedAt = DateTime.UtcNow;

                var transaction = _mapper.Map<Transaction>(transactionDto);
                transaction.CreatedAt = DateTime.UtcNow;

                await _transactionRepository.AddAsync(transaction);
                await _walletRepository.UpdateAsync(wallet);

                return _mapper.Map<TransactionReadDto>(transaction);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found for WalletId {WalletId}", transactionDto.WalletId);
                throw; 
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid transaction amount: {Error}", ex.Message);
                throw; 
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Transaction failed: {Error}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw new ApplicationException("An unexpected error occurred while creating the transaction.", ex);
            }
        }



        public async Task<IEnumerable<TransactionReadDto>> GetTransactionsAsync(int walletId)
        {
            try
            {
                var transactions = await _transactionRepository.GetByWalletIdAsync(walletId);
                return _mapper.Map<IEnumerable<TransactionReadDto>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for WalletId {WalletId}", walletId);
                throw new ApplicationException("An error occurred while retrieving the transactions.");
            }
        }










    }

}
