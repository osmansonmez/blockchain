using Nethereum.Web3.Accounts.Managed;
using Nethereum.Contracts;
using Nethereum.Contracts.CQS;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Hex;
using Nethereum.Hex.HexConvertors;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.ABI;

using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

using ContractInterface.Common.Entities;
using ContractInterface.Common.Helpers;

namespace ContractInterface.Common
{
    public partial class ContractFacade: IContractFacade
    {


        /// <summary>
        /// Deploy smart contract, other than contract's abi, bytecode, sender address,
        /// it must include any parameters for contract's constructor. 
        /// </summary>
        /// <param name="name">Contract name</param>
        /// <param name="abi"></param>
        /// <param name="byteCode"></param>
        /// <param name="senderAddress">Contract owner</param>
        /// <param name="senderPassword"></param>
        /// <param name="gas"></param>
        /// <returns></returns>
        public async Task<DeploymentResult> Deploy(
            string name, 
            string abi, 
            string byteCode, 
            string senderAddress, 
            string senderPassword, 
            BigInteger gas)
        {   
            // var isNameUnique = CheckUniqueContractName(name);
            // if(!isNameUnique)
            //     return new DeployResponse(){
            //     Success = false,
            //     StatusMessage = "Name exists. Please choose another name"};
                
            var account = new ManagedAccount(senderAddress, senderPassword);
            var web3 = new Web3(account, Config.GetSection(Constants.GETH_RPC).Value);
            
            string transactionHash = null;
            try
            {
                transactionHash = await web3.Eth.DeployContract
                                                .SendRequestAsync(abi, byteCode, senderAddress, new HexBigInteger(gas));
            }
            catch(Exception ex)
            {
                Logger.LogError("Deployment failed!\n" + ex.StackTrace);
                return new DeploymentResult(){Success = false, StatusMessage="Deployment failed!"};
            }

            var transactionReceipt = await TryGetReceipt(web3, transactionHash);
            
            string contractAddress = null;
            try
            {
                contractAddress = transactionReceipt.ContractAddress;
            }
            catch(Exception ex)
            {
                Logger.LogError("Error retrieving address: \n" + ex.StackTrace);
            }

            var contract = await Task.Run(()=> web3.Eth.GetContract(abi, contractAddress));
            ContractDAO contractDAO = new ContractDAO(){
                Contract = contract,
                Name = name,
                Address = contractAddress,
                Abi = abi,
                CodeHash = byteCode,
                StorageHash = byteCode,
                CreatedDateTime = DateTime.Now.ToUniversalTime()
            };
            await Task.Run(()=> SaveContractToXml(contractDAO, name));
            
            List<ContractDAO> contractList = null;
            if(!Cache.TryGetValue(Constants.CACHE_CONTRACT_LIST, out contractList))
                contractList = new List<ContractDAO>();

            contractList.Add(contractDAO);
            Cache.Set(Constants.CACHE_CONTRACT_LIST, contractList);

            return new DeploymentResult(){
                ContractAddress  = contractAddress,
                TransactionReceipt = transactionReceipt,
                TransactionHash = transactionHash,
                DeployedAtBlock = transactionReceipt.BlockNumber,
                OwnerAddress = senderAddress,
                GasPrice = transactionReceipt.GasUsed,
                Success = true
            };
        }


        public async Task<TransactionResult> TryGetReceipt(Web3 web3, string transactionHash)
        {
            TransactionReceipt transactionReceipt = null;
            int retry = 10;
            while(retry > 0 && transactionReceipt ==null)
            {
                try
                {
                    transactionReceipt = await web3.Eth.Transactions
                                                .GetTransactionReceipt
                                                .SendRequestAsync(transactionHash);
                }
                catch
                {
                    retry--;
                    if(retry <= 0)
                    {
                        Logger.LogError("Failed to get transaction receipt.");
                        return null;
                    }
                }
            }   

            return new TransactionResult(transactionReceipt);
        }

        public void SaveContractToXml(ContractDAO contract, string contractName)
        {
            var path = GetArtifactDir();

            try
            {
                var xdoc = new XmlDocument();
                xdoc.Load(path);
                XmlElement root = xdoc.DocumentElement;
                if(root == null)
                {
                    root = xdoc.CreateElement("ContractArtifacts");
                    xdoc.AppendChild(root);
                }
                
                var contractElm = xdoc.CreateElement("Contract");
                root.AppendChild(contractElm);
                
                contractElm.SetAttribute("Name", contractName);
                
                var ctAddress = xdoc.CreateElement("Address");
                ctAddress.InnerText = contract.Address.ToString();
                contractElm.AppendChild(ctAddress);

                var createdTime = xdoc.CreateElement("CreatedDateTime");
                var timestamp = contract.CreatedDateTime.ToUniversalTime();
                createdTime.InnerText = $"{timestamp.ToLongTimeString()} {timestamp.ToLongDateString()}, UTC";
                contractElm.AppendChild(createdTime);

                xdoc.Save(path);
            } 
            catch(Exception ex)
            {
                Logger.LogError("An error occur while writing contract to file: \n" + ex.StackTrace);
            }
            
        }

        public bool CheckUniqueContractName(string name)
        {
            var path = GetArtifactDir();
            var xdoc = new XmlDocument();
            if(!File.Exists(path))
                return true;
            xdoc.Load(path);
            var xml = XElement.Parse(xdoc.InnerXml);
            var contracts = xml?.Elements("Contract");
            var found = contracts?.Where(x => x.Attribute("Name").Equals(name));
            if(found == null || !found.Any())
                return true;
            return false;
        }

        private string GetArtifactDir()
        {
            var dir = Directory.GetCurrentDirectory();
            var contractDir = Directory.CreateDirectory($"{dir}/Contracts");
            var path = $"{contractDir}/{Constants.CONTRACT_ARTIFACT_FILE_NAME}.xml";
            return path;
        }
        
    }
}