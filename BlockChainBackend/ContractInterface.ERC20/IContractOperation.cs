using System.Numerics;
using System.Threading.Tasks;

using Nethereum.Contracts;
using Nethereum.Web3;

using ContractInterface.Common.Entities;
using ContractInterface.Common.Helpers;

namespace ContractInterface.ERC20
{
    public interface IContractOperation
    {
        Task<BigInteger> GetTotalSupply(Contract contract);
        Task<string> GetOwner(Contract contract);
        Task<BigInteger> GetBalanceOf(Contract contract, string address);
        Task<BigInteger> Allowance(Contract contract, string sender, string tokenOwner, string appointee);
        Task<TransactionResult> RemoveAppointee(Contract contract, Web3 web3, string sender, string addressToRemove);
        Task<TransactionResult> RemoveHolder(Contract contract, Web3 web3, string sender, string addressToRemove);
        Task<TransactionResult> AddHolder(Contract contract, Web3 web3, string sender, string addressToAdd);
        Task<TransactionResult> Approve(Contract contract, Web3 web3, string sender, string appointee, BigInteger withdrawAmount);
        Task<TransactionResult>  Transfer(Contract contract, Web3 web3, string sender, string fromAddress, string toAddress, BigInteger transferAmount);
        Task<bool> CheckAppointeePermission(Contract contract, string toCheck, string mapToOwner);
        Task<bool> CheckOwnerPermission(Contract contract, string toCheck);
        Task<bool> CheckHolderPermission(Contract contract, string toCheck);
        Task<bool> HolderExist(Contract contract, string toCheck);
        Task<TransactionResult> GenericTransaction(Contract contract, Web3 web3, string sender, string functionName, params object[] inputs);

    }
}