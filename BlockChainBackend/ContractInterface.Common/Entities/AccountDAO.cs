using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Util;

namespace ContractInterface.Common.Entities
{
    [FunctionOutput]
    public class AccountDAO
    {
        [Parameter("address", 1)]
        public string Address{get; set;}
        public BigDecimal Balance{get; set;}
        public long Nonce{get; set;}
        public string PublicKey{get; set;}
        public string Password{get; set;}
        public string Token{get; set;}
        
    }
}