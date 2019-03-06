using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.Hex.HexConvertors;

using ContractInterface.Common;
using ContractInterface.Common.Helpers;

namespace ContractInterface.ERC20
{
    public partial class ContractOperation: IContractOperation
    {

        #region Write
        public async Task<TransactionResult>  Transfer(
            Contract contract, 
            Web3 web3, 
            string sender, 
            string fromAddress, 
            string toAddress, 
            BigInteger transferAmount)
        {
            return await GenericTransaction(contract, web3, sender, Constants.FUNCTION_TRANSFER_FROM, new object[]{fromAddress, toAddress, transferAmount});
        }

        public async Task<TransactionResult> Approve(Contract contract, Web3 web3, string sender, string appointee, BigInteger withdrawAmount)
        {
            return await GenericTransaction(contract, web3, sender, Constants.FUNCTION_APPROVE, new object[]{appointee,withdrawAmount});
        }

        public async Task<TransactionResult> AddHolder(Contract contract, Web3 web3, string sender, string addressToAdd)
        {
            var funcName = Constants.FUNCTION_ADD_HOLDER;
            return await GenericTransaction(contract, web3, sender, funcName, new string[]{addressToAdd} );

        }

        public async Task<TransactionResult> RemoveHolder(Contract contract, Web3 web3, string sender, string addressToRemove)
        {
            var funcName = Constants.FUNCTION_REMOVE_HOLDER;
            return await GenericTransaction(contract, web3, sender, funcName, new string[]{addressToRemove});
        }

        public async Task<TransactionResult> RemoveAppointee(Contract contract, Web3 web3, string sender, string addressToRemove)
        {
            var funcName = Constants.FUNCTION_REMOVE_APPOINTEE;
            return await GenericTransaction(contract, web3, sender, funcName, new string[]{addressToRemove});
        }

        #endregion


        #region Read
        public async Task<BigInteger> Allowance(Contract contract, string sender, string tokenOwner, string appointee)
        {
            var allowanceFunc = contract.GetFunction(Constants.FUNCTION_ALLOWANCE);
            return await allowanceFunc.CallAsync<BigInteger>(new string[]{tokenOwner, appointee});
        }
        public async Task<bool> HolderExist(Contract contract, string toCheck)
        {
            var funcName = Constants.FUNCTION_HOLDER_EXIST;
            return await contract.GetFunction(funcName).CallAsync<bool>(new string[]{toCheck} );
        }
        public async Task<bool> CheckHolderPermission(Contract contract, string toCheck)
        {
            var funcName = Constants.FUNCTION_CHECK_HOLDER_PERMISSION;
            return await contract.GetFunction(funcName).CallAsync<bool>(new string[]{toCheck} );
        }
        public async Task<bool> CheckOwnerPermission(Contract contract, string toCheck)
        {
            var funcName = Constants.FUNCTION_CHECK_OWNER_PERMISSION;
            return await contract.GetFunction(funcName).CallAsync<bool>(new string[]{toCheck} );
        }
        public async Task<bool> CheckAppointeePermission(Contract contract, string toCheck, string mapToOwner)
        {
            var funcName = Constants.FUNCTION_CHECK_APPOINTEE_PERMISSION;
            return await contract.GetFunction(funcName).CallAsync<bool>(new string[]{toCheck, mapToOwner} );
        }

        public async Task<string> GetOwner(Contract contract)
        {
            var funcName = Constants.FUNCTION_GET_OWNER;
            return await contract.GetFunction(funcName).CallAsync<string>();
        }

        #endregion

        public async Task<TransactionResult> GenericTransaction(Contract contract, Web3 web3, string sender, string functionName, params object[] inputs)
        {
            var func = contract.GetFunction(functionName);
            string txHash = null;
            try
            {
                txHash = await func.SendTransactionAsync(sender, new HexBigInteger(Constants.DEFAULT_GAS), new HexBigInteger(Constants.DEFAULT_VALUE), inputs);
            }
            catch (Nethereum.JsonRpc.Client.RpcResponseException)
            {
                throw;
            }
            return await ContractFacade.TryGetReceipt(web3, txHash);
        }

    }
}