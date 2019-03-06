using System.Threading.Tasks;
using System.Threading;
using System.Numerics;

using Nethereum.Web3;

using ContractInterface.Common.Helpers;
using ContractInterface.Common.Entities;

namespace ContractInterface.Common
{
    public interface IContractFacade
    {
        Task<DeploymentResult> Deploy(string name, string abi, string byteCode, string senderAddress, string senderPassword, BigInteger gas);
        Task<TransactionResult> TryGetReceipt(Web3 web3, string transactionHash);
        Task<ContractDAO> GetContract(string contractName, bool isDeployed, string contractAddress);
        Web3 GetWeb3(string address, string password, string rpcUrl);
        Task<string> GetByteCode(string contractName, bool isDeployed, string contractAddress = null);

        Task<string> GetAbi(string contractName, bool isDeployed, string contractAddress = null);

    }
}