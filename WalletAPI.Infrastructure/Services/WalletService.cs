using AutoMapper;
using Microsoft.Extensions.Logging;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Application.Contracts.Services;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain;

namespace WalletAPI.Infrastructure.Services
{
    public class WalletService:IWalletService
    {
        private readonly IWalletRespository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<WalletService> _logger;

        public WalletService(IWalletRespository repository, IMapper mapper, ILogger<WalletService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Wallet> CreateWalletAsync(WalletWriteDto walletDto)
        {
            try
            {
                var wallet = _mapper.Map<Wallet>(walletDto);
                wallet.CreatedAt = DateTime.UtcNow;
                wallet.UpdatedAt = DateTime.UtcNow;

                await _repository.AddAsync(wallet);
                return wallet;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wallet");
                throw new ApplicationException("An error occurred while creating the wallet.");
            }
        }


        public async Task<WalletReadDto> GetWalletAsync(int id)
        {
            try
            {
                var wallet = await _repository.GetByIdAsync(id);
                if (wallet == null)
                    throw new KeyNotFoundException("Wallet not found.");

                return _mapper.Map<WalletReadDto>(wallet);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found: {WalletId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wallet with ID {WalletId}", id);
                throw new ApplicationException("An error occurred while retrieving the wallet.");
            }
        }



        public async Task UpdateWalletAsync(int id, WalletWriteDto walletDto)
        {
            try
            {
                var wallet = await _repository.GetByIdAsync(id);
                if (wallet == null)
                    throw new KeyNotFoundException("Wallet not found.");

                _mapper.Map(walletDto, wallet);
                wallet.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(wallet);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Wallet not found for update: {WalletId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating wallet with ID {WalletId}", id);
                throw new ApplicationException("An error occurred while updating the wallet.");
            }
        }


        public async Task DeleteWalletAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting wallet with ID {WalletId}", id);
                throw new ApplicationException("An error occurred while deleting the wallet.");
            }
        }



    }
}
