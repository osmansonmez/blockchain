using System;

namespace BlockChainBackend.Models.Customer
{
    public class CreateCheckRequest : BaseCustomerRequestViewModel
    {
        /// <summary>
        /// MBB, TCKN or Ether Address, you can send any identity number that you have
        /// </summary>
        public string ToUser { get; set; }
        public DateTime CheckDate { get; set; }
        public int Amount { get; set; }
    }
}