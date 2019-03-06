using System;
using System.Numerics;

using Nethereum.Hex.HexTypes;
using Nethereum.Hex.HexConvertors;
using Nethereum.RPC.Eth.DTOs;

namespace ContractInterface.Common.Helpers
{
    public class TransactionResult
    {
        
        public string TransactionHash { get; set; }
        public BigInteger TransactionIndex { get; set; }
        public string BlockHash { get; set; }
        public BigInteger BlockNumber { get; set; }
        public BigInteger CumulativeGasUsed { get; set; }
        public BigInteger GasUsed { get; set; }
        public string ContractAddress { get; set; }
        public string Status { get; set; }
        public string[] Logs { get; set; }

        public TransactionResult()
        {

        }
        public TransactionResult(TransactionReceipt receipt)
        {
            TransactionHash = receipt.TransactionHash;
            TransactionIndex = receipt.TransactionIndex.Value;
            BlockHash = receipt.BlockHash;
            BlockNumber = receipt.BlockNumber.Value;
            CumulativeGasUsed = receipt.CumulativeGasUsed.Value;
            GasUsed = receipt.GasUsed.Value;
            ContractAddress = receipt.ContractAddress;
            if(receipt.Status.Value == 1) 
                Status = "Transaction succeeded."; 
            else 
                Status = "Transaction failed!";                
        }
    }
}