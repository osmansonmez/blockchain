using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BlockChainBackend.Models;
using BlockChainBackend.Models.Bank;
using ContractInterface.Common.Entities;
using ContractInterface.ERC20;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Utilities.Encoders;
using BlockChainBackend.Models.WalletModels;
using BlockChainBackend.Services;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ContractInterface.Common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private IConfiguration _config;
        private ILogger<BankController> _logger;
        private IWalletService _walletService;
        private ICheckService _checkService;
        public BankController(IConfiguration configuration,
            IWalletService walletService,
            ICheckService checkService,
            ILogger<BankController> logger)
        {
            _config = configuration;
            _logger = logger;
            _walletService = walletService;
            _checkService = checkService;
        }

        [HttpPost]
        public async Task<IActionResult> QueryCustomerAdress(QueryCustomerAdressRequestModel request)
        {
            var address = await _walletService.GetWalletAdress(request.CustomerId);
            AccountModel accountModel = new AccountModel()
            {
                CustomerId = request.CustomerId,
                EthereumAdress = address,
                Amounts = new List<Amount>()
            };
            
            return await Task.FromResult(Ok(new QueryCustomerAdressResponseModel(){Account = accountModel}));
        }

        
        [HttpPost]
        public async Task<IActionResult> AddCustomerToWallet(AddCustomerToWalletRequestModel request)
        {
            var address = await _walletService.GetWalletAdress(request.CustomerId);
            var addr = address?.RemoveHexPrefix().Trim('0');
            if (!string.IsNullOrEmpty(addr))
            {
                AccountModel accModel = new AccountModel()
                {
                    CustomerId = request.CustomerId,
                    EthereumAdress = address,
                    Amounts = new List<Amount>()
                };
                
                return await Task.FromResult(Ok(new AddCustomerToWalletResponseModel(){Account = accModel}));
            }

            address = await _walletService.CreateAndAddAdressToWallet(request.CustomerId);
            
            AccountModel accountModel = new AccountModel()
            {
                CustomerId = request.CustomerId,
                EthereumAdress = address,
                Amounts = new List<Amount>()
            };
            
             return await Task.FromResult(Ok(new AddCustomerToWalletResponseModel(){Account = accountModel}));
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerToCheckAccount(AddCustomerToCheckAccountRequest request)
        {
            bool result = await _checkService.AddCheckCustomer(request.CustomerId);
            return await Task.FromResult(Ok(new AddCustomerToCheckAccountResponse()));
        }
        
        [HttpPost]
        public async Task<IActionResult> IsCustomerHasCheckAccount(IsCustomerHasCheckAccountRequest request)
        {
            bool result = await _checkService.IsCheckCustomer(request.CustomerId);
            return await Task.FromResult(Ok(new IsCustomerHasCheckAccountResponse(){HasCheckAccount = result}));
        }
    }
}