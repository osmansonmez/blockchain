using System.Collections.Generic;

namespace BlockChainBackend.Models
{
    public class AccountModel
    {
        public string CustomerId { get; set; }
        
        public string EthereumAdress { get; set; }
        
        public List<Amount>  Amounts { get; set; }
    }

    public class Amount
    {
        public string CurrencyName { get; set; }
        
        public decimal Value { get; set; } 
    }
}