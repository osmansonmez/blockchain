using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BlockChainBackend.Models.WalletModels;
using ContractInterface.Common;
using ContractInterface.Common.Entities;
using ContractInterface.ERC20;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using Org.BouncyCastle.Math;
using BigInteger = System.Numerics.BigInteger;
using Constants = ContractInterface.Common.Constants;

namespace BlockChainBackend.Services
{
    public class WalletService : IWalletService
    {
        private IConfiguration _config;
        private IContractFacade _contracts;
        private ILogger<WalletService> _logger;
        private IWeb3Backend _web3Backend;
        private ManagedAccount _account;
        private Web3 _web3;
        private ContractDAO _walletcontract;
        public WalletService(IConfiguration configuration, IContractFacade contracts,
            ILogger<WalletService> logger, IWeb3Backend web3Backend)
        {
            _config = configuration;
            _contracts = contracts;
            _logger = logger;
            _web3Backend = web3Backend;
        }

        public async Task<string> GetWalletAdress(string CustomerId)
        {
             _walletcontract = _walletcontract ?? await _contracts.GetContract("Wallet", _config.GetSection("NetworkId").Value);
            _account = _account ?? _web3Backend.GetDefaultAccount();
            _web3 = _web3 ?? _web3Backend.GetDefaultWeb3();

            GetCustomerAddressInputFunction customeradressFunction = new GetCustomerAddressInputFunction()
            {
                customerId = CustomerId,
                FromAddress = _account.Address,
            };

            var handler = _web3.Eth.GetContractQueryHandler<GetCustomerAddressInputFunction>();
            var result = await
                handler.QueryDeserializingToObjectAsync<GetCustomerAddressOutputFunction>(
                    customeradressFunction,
                    _walletcontract.Address);
            var adress = result?.Address;
            return await Task.FromResult(adress);
        }

        public async Task<string> CreateAndAddAdressToWallet(string CustomerId)
        {
            _account = _account ?? _web3Backend.GetDefaultAccount();
            _web3 = _web3 ?? _web3Backend.GetDefaultWeb3();

            _walletcontract = _walletcontract ?? await _contracts.GetContract("Wallet", _config.GetSection("NetworkId").Value);
            var newAccount =await _web3.Personal.NewAccount.SendRequestAsync("");
           /*
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var newAccount = (new Nethereum.Web3.Accounts.Account(privateKey)).Address;
               */ 
            AddToWalletInputFunction func = new AddToWalletInputFunction()
            {
                customerId = CustomerId,
                FromAddress = _account.Address,
                newAccount = newAccount,
                Gas = Constants.DEFAULT_GAS,
                GasPrice = Constants.DEFAULT_GAS_PRICE
            };

            var addtowalletHandler = _web3.Eth.GetContractTransactionHandler<AddToWalletInputFunction>();
            var addtoWalletResult =
                await addtowalletHandler.SendRequestAndWaitForReceiptAsync(_walletcontract.Address, func);
            if (addtoWalletResult.Status == BigInteger.One)
            {
                var transferEther =
                    await _web3.TransactionManager.TransactionReceiptService.SendRequestAndWaitForReceiptAsync(
                        new TransactionInput()
                            {From = _account.Address, To = newAccount, Value = new HexBigInteger(1000000000000000000)},
                        null);
            }

            return await Task.FromResult(newAccount);
        }
    }
}