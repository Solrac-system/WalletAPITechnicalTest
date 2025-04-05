using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Application.Contracts.Services;
using WalletAPI.Infrastructure.Data;
using WalletAPI.Infrastructure.Repository;
using WalletAPI.Infrastructure.Services;

namespace WalletAPI.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastuctureService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<WalletDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWalletRespository, WalletRepository>();
            services.AddScoped<ITransactionRespository,TransactionRepository>();

            return services;
        }
    }
}
