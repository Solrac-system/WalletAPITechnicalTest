using WalletAPI.Domain;

namespace WalletAPI.Application.Contracts.Persistence
{
    public interface ITransactionRespository
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(int walletId);
    }
}
