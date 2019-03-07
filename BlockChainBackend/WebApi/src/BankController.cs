using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

namespace ContractInterface.Common
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private IConfiguration _config;
        private IContractFacade _contracts;
        private IContractOperation _operation;
        private ILogger<BankController> _logger;
        public BankController(IConfiguration configuration,IContractFacade contracts,IContractOperation operation,
            ILogger<BankController> logger)
        {
            _config = configuration;
            _account = GetDefaultAccount();
            _web3 = GetDefaultWeb3();
            _contracts = contracts;
            _operation = operation;
            _logger = logger;
        }

        private ManagedAccount _account;
        private Web3 _web3;

        [Function("AddToWallet")]
        public class AddToWalletFunction : FunctionMessage
        {
            [Parameter("string", "customerId", 1)]
            public string customerId { get; set; }

            [Parameter("address", "addr", 2)]
            public string newAccount { get; set; }
        }
        
        [Function("GetCustomerAddress","string")]
        public class GetCustomerAddressFunction : FunctionMessage
        {
            [Parameter("string", "customerId", 1)]
            public string customerId { get; set; }
        }

        [FunctionOutput]
        public class GetCustomerAddressFunctionOutput : IFunctionOutputDTO
        {
            [Parameter("string", "addr", 1)]
            public string Adress { get; set; }
        }
        

        [HttpPost]
        public async Task<string> QueryCustomerAdress(string customerId)
        {
            var walletcontract = await _contracts.GetContract("Wallet", _config.GetSection("NetworkId").Value);
            GetCustomerAddressFunction customeradressFunction = new GetCustomerAddressFunction()
            {
                customerId = customerId,
                FromAddress = "0xe89a8f9f34c6ec53ac9a963fb07d3327921a2def",

            };

          var handler =  _web3.Eth.GetContractQueryHandler<GetCustomerAddressFunction>();
            var result = await
                handler.QueryRawAsync(walletcontract.Address,customeradressFunction);
            return result;
        }

        [HttpPost]
        public async Task<bool> AddCustomer(string customerId)
        {
          var  newAccount = _web3.Personal.NewAccount.SendRequestAsync("").Result;
          var walletcontract = await _contracts.GetContract("Wallet",_config.GetSection("NetworkId").Value);
    
            
            
            /*
            AddToWalletFunction func = new AddToWalletFunction()
            {
                customerId = customerId,
                FromAddress = "0xe89a8f9f34c6ec53ac9a963fb07d3327921a2def",
                newAccount = newAccount
            };
            
            var addtowalletHandler =  _web3.Eth.GetContractTransactionHandler<AddToWalletFunction>();

           var addtoWalletResult =  await addtowalletHandler.SendRequestAndWaitForReceiptAsync(walletcontract.Address, func);
            
         */
            
            var walletresult =  await _operation.GenericTransaction(walletcontract.Contract, _web3, 
              "0xe89a8f9f34c6ec53ac9a963fb07d3327921a2def",  "AddToWallet", customerId, newAccount);
            
            var getaddr =  await _operation.GenericTransaction(walletcontract.Contract, _web3, 
                "0xe89a8f9f34c6ec53ac9a963fb07d3327921a2def",  "GetCustomerAddress", customerId);
            
          var contract = await _contracts.GetContract("PostdatedCheckManager",_config.GetSection("NetworkId").Value);
           var result =  await _operation.GenericTransaction(contract.Contract, _web3, "0xe89a8f9f34c6ec53ac9a963fb07d3327921a2def",
                "addCustomer", newAccount);
            return await Task.FromResult(true);
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<AccountDAO> accounts = new List<AccountDAO>();
            var accs = _web3.Personal.ListAccounts.SendRequestAsync().Result;
            foreach (var item in accs)
            {
               
                var balance = await _web3.Eth.GetBalance.SendRequestAsync(item);
                var etherAmount = Web3.Convert.FromWeiToBigDecimal(balance.Value);
                accounts.Add(new AccountDAO(){Address = item,Balance = etherAmount});
            }
            return Ok(accounts);
        }

        private ManagedAccount GetDefaultAccount()
        {
            return new ManagedAccount(Constants.DEFAULT_TEST_ACCOUNT_ADDRESS, Constants.DEFAULT_TEST_ACCOUNT_PASSWORD);
        }

        private Web3 GetDefaultWeb3()
        {
            return new Web3(_config.GetSection(Constants.GETH_RPC).Value);
        }
    }
}