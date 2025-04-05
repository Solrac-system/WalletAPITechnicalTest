using WalletAPI.Application.DTOs;

namespace WalletAPI.Application.Contracts.Services
{
    public interface ITransactionService
    {
        Task<TransactionReadDto> CreateTransactionAsync(TransactionWriteDto transactionDto);
        Task<IEnumerable<TransactionReadDto>> GetTransactionsAsync(int walletId);
    }
}
