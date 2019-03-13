using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace BlockChainBackend.Models.WalletModels
{
    public class AkTLModels
    {
        /// <summary>
        /// AK TL YOLLA
        /// </summary>
        [Functgit addion("AddToWallet")]
        public class AddToWalletInputFunction : FunctionMessage
        {
            [Parameter("string", "customerId", 1)] public string customerId { get; set; }

            [Parameter("address", "addr", 2)] public string newAccount { get; set; }
        }
    }
}