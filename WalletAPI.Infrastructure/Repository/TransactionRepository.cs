using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Domain;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Infrastructure.Repository
{
    public class TransactionRepository(WalletDbContext context) : ITransactionRespository
    {
        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(int walletId)
        {
            return await context.Transaction.Where(t => t.WalletId == walletId).ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await context.Transaction.AddAsync(transaction);
            await context.SaveChangesAsync();
        }

    }
}
