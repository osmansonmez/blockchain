using System;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace BlockChainBackend.Models.WalletModels
{
    public class CheckModels
    {
        /// <summary>
        /// Müşteriyi Check contcrat'a eklemek için kullanılır
        /// </summary>
        [Function("addCustomer")]
        public class AddCustomerInputFunction : FunctionMessage
        {
            [Parameter("address", "customer", 1)] public string Address { get; set; }
        }

        /// <summary>
        /// Müşteriyi check müşterisi mi bilgisinin input requesti
        /// </summary>
        [Function("isCustomer", "bool")]
        public class IsCustomerInputFunction : FunctionMessage
        {
            [Parameter("address", "customer", 1)] public string Address { get; set; }
        }

        /// <summary>
        /// Müşteriyi check müşterisi mi bilgisinin input requesti
        /// </summary>
        [FunctionOutput]
        public class IsCustomerOutputModel : IFunctionOutputDTO
        {
            [Parameter("bool", null, 1)] public bool IsCustomer { get; set; }
        }

        /// <summary>
        /// Create Check Request
        /// </summary>
        [Function("createCheck")]
        public class CreateCheckInputFunction : FunctionMessage
        {
            [Parameter("address", "to", 1)] public string To { get; set; }
            [Parameter("uint256", "date", 2)] public long CheckDate { get; set; }
            [Parameter("uint256", "amount", 3)] public uint Amount { get; set; }
        }

        [Event("LogCheck")]
        public class CreateCheckEventDTO : IEventDTO
        {
            [Parameter("address", "customer", 1, true)]
            public string From { get; set; }

            [Parameter("uint256", "tokenId", 2, true)]
            public int CheckId { get; set; }
        }

        /// <summary>
        /// CheckInfo Request
        /// </summary>
        [Function("checkInfo")]
        public class CheckInfoInputFunction : FunctionMessage
        {
            [Parameter("uint256", "tokenId", 1)] public long TokenId { get; set; }
        }

        /// <summary>
        /// CheckInfo Output
        /// </summary>
        [FunctionOutput]
        public class CheckInfoOutputModel : IFunctionOutputDTO
        {
            [Parameter("address", "creator", 1)] public string Creator { get; set; }

            [Parameter("uint256", "date", 2)] public long CheckDate { get; set; }

            [Parameter("uint256", "amount", 3)] public long Amount { get; set; }
            
            [Parameter("address", "owner", 4)] public string Owner { get; set; }

        }
        
        /// <summary>
        /// CheckInfo Request
        /// </summary>
        [Function("tokensOfOwner")]
        public class CheckListOwnerInputFunction : FunctionMessage
        {
            [Parameter("address", "owner", 1)] public string Address { get; set; }
        }
        
        /// <summary>
        /// CheckInfo Request
        /// </summary>
        [Function("tokensOfCustomer")]
        public class CheckListCreatedInputFunction : FunctionMessage
        {
            [Parameter("address", "customer", 1)] public string Address { get; set; }
        }

        /// <summary>
        /// CheckInfo Output
        /// </summary>
        [FunctionOutput]
        public class CheckListOutputModel : IFunctionOutputDTO
        {
            [Parameter("uint256[]", null, 1)] public List<long> CheckList { get; set; } = null;
        }
    }
}