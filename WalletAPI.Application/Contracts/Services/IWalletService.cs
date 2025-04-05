using WalletAPI.Application.DTOs;
using WalletAPI.Domain;

namespace WalletAPI.Application.Contracts.Services
{
    public interface IWalletService
    {
        Task<Wallet> CreateWalletAsync(WalletWriteDto walletDto);
        Task DeleteWalletAsync(int id);
        Task<WalletReadDto> GetWalletAsync(int id);
        Task UpdateWalletAsync(int id, WalletWriteDto walletDto);
    }
}
