using System;

namespace BlockChainBackend.Models.Customer
{
    public class CheckInfoResponse : BaseCustomerResponseViewModel
    {
        public Check Check { get; set; }
    }
}