using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Domain;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Infrastructure.Repository
{
    public class WalletRepository : IWalletRespository
    {
        private readonly WalletDbContext _context;
        public WalletRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetByIdAsync(int id)
        {
            return await _context.Wallet.FindAsync(id);
        }

        public async Task<IEnumerable<Wallet>> GetAllAsync()
        {
            return await _context.Wallet.ToListAsync();
        }

        public async Task AddAsync(Wallet wallet)
        {
            await _context.Wallet.AddAsync(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallet.Update(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wallet = await GetByIdAsync(id);
            if (wallet != null)
            {
                _context.Wallet.Remove(wallet);
                await _context.SaveChangesAsync();
            }
        }


    }
}
