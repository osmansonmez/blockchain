using System.Collections.Generic;
using System.Threading.Tasks;
using ContractInterface.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using Newtonsoft.Json.Serialization;

namespace ContractInterface.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IConfiguration _config;

        public AccountsController(IConfiguration configuration)
        {
            _config = configuration;
            _account = GetDefaultAccount();
            _web3 = GetDefaultWeb3();
        }

        private ManagedAccount _account;
        private Web3 _web3;

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

        public Web3 GetDefaultWeb3()
        {
            return new Web3(_config.GetSection(Constants.GETH_RPC).Value);
        }
    }
}