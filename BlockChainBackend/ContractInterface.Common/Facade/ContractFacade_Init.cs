using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;


using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.Web3.Accounts.Managed;

namespace ContractInterface.Common
{
    public partial class ContractFacade: IContractFacade
    {
        protected IConfiguration Config;
        protected readonly ILogger Logger;
        protected IMemoryCache Cache;
        private readonly object _locker = new object();
        public ContractFacade(IConfiguration config, ILogger<ContractFacade> logger, IMemoryCache cache)
        {
            Config = config;
            Logger = logger;
            Cache =  cache;
        }
    }
}