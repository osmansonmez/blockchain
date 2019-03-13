using Nethereum.Web3;
using Nethereum.Web3.Accounts.Managed;

namespace BlockChainBackend.Services
{
    public interface IWeb3Backend
    {
        ManagedAccount GetAccount(string EtherAddress);
        ManagedAccount GetDefaultAccount();
        Web3 GetDefaultWeb3();
        Web3 GetWeb3(ManagedAccount account);
    }
}