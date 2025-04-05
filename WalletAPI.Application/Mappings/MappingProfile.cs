using AutoMapper;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain;

namespace WalletAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Wallet, WalletReadDto>();
            CreateMap<WalletWriteDto, Wallet>();

            CreateMap<Transaction, TransactionReadDto>();
            CreateMap<TransactionWriteDto, Transaction>();



        }
    }
    
}
