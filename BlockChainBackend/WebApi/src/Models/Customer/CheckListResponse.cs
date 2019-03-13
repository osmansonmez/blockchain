using System.Collections.Generic;
using BlockChainBackend.Models;
using BlockChainBackend.Models.Customer;

namespace BlockChainBackend.Services
{
    public class CheckListResponse : BaseCustomerResponseViewModel
    {
       public List<Check> CheckList { get; set; }  
    }
}