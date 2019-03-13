using ContractInterface.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;

namespace BlockChainBackend.Services
{
    public class Web3Backend : IWeb3Backend
    {
        private IConfiguration _config;
        private ILogger<Web3Backend> _logger;

        public Web3Backend(IConfiguration configuration,
            ILogger<Web3Backend> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        public ManagedAccount GetAccount(string EtherAddress)
        {
            return new ManagedAccount(EtherAddress, Constants.DEFAULT_TEST_ACCOUNT_PASSWORD);
        }

        public ManagedAccount GetDefaultAccount()
        {
            return new ManagedAccount(Constants.DEFAULT_TEST_ACCOUNT_ADDRESS, Constants.DEFAULT_TEST_ACCOUNT_PASSWORD);
        }

        public Web3 GetDefaultWeb3()
        {
            return new Web3(_config.GetSection(Constants.GETH_RPC).Value);
        }

        public Web3 GetWeb3(ManagedAccount account)
        {
            return new Web3(account, _config.GetSection(Constants.GETH_RPC).Value);
        }
    }
}