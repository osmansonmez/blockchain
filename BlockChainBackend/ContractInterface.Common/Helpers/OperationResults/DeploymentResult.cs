using System;
using System.Numerics;

using Nethereum.RPC.Eth.DTOs;

namespace ContractInterface.Common.Helpers
{
    public class DeploymentResult
    {
        public string ContractAddress{get; set;}
        public string OwnerAddress{get; set;}
        public BigInteger GasPrice{get; set;}
        public string TransactionHash{get; set;}
        public TransactionResult TransactionReceipt{get; set;}
        public BigInteger DeployedAtBlock{get; set;}

        public bool Success{get; set;}
        public string StatusMessage{get; set;}
    }
}