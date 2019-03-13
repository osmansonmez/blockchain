using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace BlockChainBackend.Models.WalletModels
{
    /// <summary>
    /// Müşteriyi Wallet contcrat'a eklemek için kullanılır
    /// </summary>
    [Function("AddToWallet")]
    public class AddToWalletInputFunction : FunctionMessage
    {
        [Parameter("string", "customerId", 1)] public string customerId { get; set; }

        [Parameter("address", "addr", 2)] public string newAccount { get; set; }
    }

    /// <summary>
    /// Wallet Contract'tan müşterinin ethereum adresini almak için kullanılır
    /// </summary>
    [Function("GetCustomerAddress", "string")]
    public class GetCustomerAddressInputFunction : FunctionMessage
    {
        [Parameter("string", "customerId", 1)] public string customerId { get; set; }
    }
    
    [FunctionOutput]
    public class GetCustomerAddressOutputFunction : IFunctionOutputDTO
    {
        [Parameter("address", "addr", 1)] public string Address { get; set; }
    }
    
}