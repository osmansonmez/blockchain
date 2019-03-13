using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlockChainBackend.Models.Customer;
using BlockChainBackend.Models.WalletModels;

namespace BlockChainBackend.Services
{
    public interface ICheckService
    {
        Task<bool> AddCheckCustomer(string CustomerId);

        Task<bool> IsCheckCustomer(string CustomerId);
        
        Task<int> CreateCheck(string fromAddress, string ToAddress, DateTime CheckDate, uint Amount);
        
        Task<CheckModels.CreateCheckEventDTO> CreatedLastCheck(string fromAddress);

        Task<CheckModels.CheckInfoOutputModel> CheckInfo(string fromAddress, long checkId);

        Task<List<Check>> CheckListCreated(string fromAddress);

        Task<List<Check>> CheckListOwner(string fromAddress);
    }
}