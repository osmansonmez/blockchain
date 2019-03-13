using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BlockChainBackend.Helpers;
using BlockChainBackend.Models.Bank;
using BlockChainBackend.Models.Customer;
using BlockChainBackend.Models.WalletModels;
using ContractInterface.Common;
using ContractInterface.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nethereum.ABI;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;
using Newtonsoft.Json.Linq;

namespace BlockChainBackend.Services
{
    public class CheckService : ICheckService
    {
        private IConfiguration _config;
        private IContractFacade _contracts;
        private ILogger<CheckService> _logger;
        private IWeb3Backend _web3Backend;
        private ManagedAccount _account;
        private Web3 _web3;
        private ContractDAO _checkcontract;
        private IWalletService _walletService;

        public CheckService(IConfiguration configuration, IContractFacade contracts,
            ILogger<CheckService> logger, IWeb3Backend web3Backend, IWalletService walletService)
        {
            _config = configuration;
            _contracts = contracts;
            _logger = logger;
            _web3Backend = web3Backend;
            _walletService = walletService;
        }

        public async Task<bool> AddCheckCustomer(string CustomerId)
        {
            var ischeckCustomer = await IsCheckCustomer(CustomerId);
            if (ischeckCustomer)
            {
                return true;
            }

            _account = _account ?? _web3Backend.GetDefaultAccount();
            _web3 = _web3 ?? _web3Backend.GetDefaultWeb3();

            var address = await _walletService.GetWalletAdress(CustomerId);

            var addr = address?.RemoveHexPrefix().Trim('0');
            if (string.IsNullOrEmpty(addr))
            {
                address = await _walletService.CreateAndAddAdressToWallet(CustomerId);
            }

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);

            CheckModels.AddCustomerInputFunction inputFunc = new CheckModels.AddCustomerInputFunction()
            {
                Address = address,
                FromAddress = _account.Address
            };

            var handler = _web3.Eth.GetContractTransactionHandler<CheckModels.AddCustomerInputFunction>();
            var result =
                await handler.SendRequestAndWaitForReceiptAsync(_checkcontract.Address, inputFunc);


            if (result.Status == BigInteger.One)
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }


        public async Task<bool> IsCheckCustomer(string CustomerId)
        {
            _account = _account ?? _web3Backend.GetDefaultAccount();
            _web3 = _web3 ?? _web3Backend.GetDefaultWeb3();

            var address = await _walletService.GetWalletAdress(CustomerId);

            var addr = address?.RemoveHexPrefix().Trim('0');
            if (string.IsNullOrEmpty(addr))
            {
                return false;
            }

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);

            CheckModels.IsCustomerInputFunction inputFunc = new CheckModels.IsCustomerInputFunction()
            {
                Address = address,
                FromAddress = _account.Address
            };

            var handler = _web3.Eth.GetContractQueryHandler<CheckModels.IsCustomerInputFunction>();
            var result =
                await handler.QueryDeserializingToObjectAsync<CheckModels.IsCustomerOutputModel>(inputFunc,
                    _checkcontract.Address);
            return await Task.FromResult(result.IsCustomer);
        }


        public async Task<int> CreateCheck(string fromAddress, string ToAddress, DateTime CheckDate, uint Amount)
        {
            _account = _account ?? _web3Backend.GetAccount(fromAddress);
            _web3 = _web3 ?? _web3Backend.GetWeb3(_account);

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);

            CheckModels.CreateCheckInputFunction inputFunc = new CheckModels.CreateCheckInputFunction()
            {
                FromAddress = fromAddress,
                To = ToAddress,
                Amount = Amount,
                CheckDate = CheckDate.ToUnixTimestampTicks()
            };

            var handler = _web3.Eth.GetContractTransactionHandler<CheckModels.CreateCheckInputFunction>();
            var result =
                await handler.SendRequestAndWaitForReceiptAsync(_checkcontract.Address, inputFunc);

            var logResult = await CreatedLastCheck(fromAddress);
            return logResult.CheckId;
        }

        public async Task<CheckModels.CreateCheckEventDTO> CreatedLastCheck(string fromAddress)
        {
            _web3 = _web3 ?? _web3Backend.GetDefaultWeb3();

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);


            var eventHandler = _web3.Eth.GetEvent<CheckModels.CreateCheckEventDTO>(_checkcontract.Address);
            var filter = eventHandler.CreateFilterInput(BlockParameter.CreateLatest(), BlockParameter.CreateLatest());
            var results = await eventHandler.GetAllChanges(filter);
            var lastEvent = results?.First().Event;
            return lastEvent;
        }

        public async Task<CheckModels.CheckInfoOutputModel> CheckInfo(string fromAddress, long checkId)
        {
            _account = _account ?? _web3Backend.GetAccount(fromAddress);
            _web3 = _web3 ?? _web3Backend.GetWeb3(_account);

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);

            CheckModels.CheckInfoInputFunction inputFunc = new CheckModels.CheckInfoInputFunction()
            {
                FromAddress = fromAddress,
                TokenId = checkId
            };

            var handler = _web3.Eth.GetContractQueryHandler<CheckModels.CheckInfoInputFunction>();
            var result =
                await handler.QueryDeserializingToObjectAsync<CheckModels.CheckInfoOutputModel>(inputFunc,_checkcontract.Address);
            return result;
        }
        
        public async Task<List<Check>> CheckListCreated(string fromAddress)
        {
            _account = _account ?? _web3Backend.GetAccount(fromAddress);
            _web3 = _web3 ?? _web3Backend.GetWeb3(_account);

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);

            CheckModels.CheckListCreatedInputFunction inputFunc = new CheckModels.CheckListCreatedInputFunction()
            {
                FromAddress = fromAddress,
                Address = fromAddress 
            };

            var handler = _web3.Eth.GetContractQueryHandler<CheckModels.CheckListCreatedInputFunction>();
            var result =
                await handler.QueryDeserializingToObjectAsync<CheckModels.CheckListOutputModel>(inputFunc,_checkcontract.Address);
            var response = new List<Check>();
            var list = result?.CheckList;
            foreach (var item in list)
            {
                var checkinfo = await CheckInfo(fromAddress, item);
                if (checkinfo != null)
                {
                    response.Add(new Check()
                    {
                        Amount = checkinfo.Amount,
                        CheckDate = TimeHelper.UnixTimestampToDateTime(checkinfo.CheckDate),
                        Creator = checkinfo.Creator,
                        Owner =  checkinfo.Owner
                    });
                }
            }

            return response;
        }
       
        public async Task<List<Check>> CheckListOwner(string fromAddress)
        {
            _account = _account ?? _web3Backend.GetAccount(fromAddress);
            _web3 = _web3 ?? _web3Backend.GetWeb3(_account);

            _checkcontract = _checkcontract ??
                             await _contracts.GetContract("PostdatedCheckManager",
                                 _config.GetSection("NetworkId").Value);

            CheckModels.CheckListOwnerInputFunction inputFunc = new CheckModels.CheckListOwnerInputFunction()
            {
                FromAddress = fromAddress,
                Address = fromAddress
            };

            var handler = _web3.Eth.GetContractQueryHandler<CheckModels.CheckListOwnerInputFunction>();
            var result =
                await handler.QueryDeserializingToObjectAsync<CheckModels.CheckListOutputModel>(inputFunc,_checkcontract.Address);

            var response = new List<Check>();
            var list = result?.CheckList;
            foreach (var item in list)
            {
                var checkinfo = await CheckInfo(fromAddress, item);
                if (checkinfo != null)
                {
                    response.Add(new Check()
                    {
                        Amount = checkinfo.Amount,
                        CheckDate = TimeHelper.UnixTimestampToDateTime(checkinfo.CheckDate),
                        Creator = checkinfo.Creator,
                        Owner =  checkinfo.Owner
                    });
                }
            }
            return response;
        }
    }
}