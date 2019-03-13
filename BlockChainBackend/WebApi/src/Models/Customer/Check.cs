using System;

namespace BlockChainBackend.Models.Customer
{
    public class Check
    {
        public long Amount { get; set; }
        
        public string Owner { get; set; }
        
        public string Creator { get; set; }
        
        public DateTime CheckDate { get; set; }
    }
}