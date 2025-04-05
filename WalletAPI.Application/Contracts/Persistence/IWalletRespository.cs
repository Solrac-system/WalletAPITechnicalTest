using WalletAPI.Domain;

namespace WalletAPI.Application.Contracts.Persistence
{
    public interface IWalletRespository
    {
        Task AddAsync(Wallet wallet);
        Task DeleteAsync(int id);
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task<Wallet> GetByIdAsync(int id);
        Task UpdateAsync(Wallet wallet);
    }
}
