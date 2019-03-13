using System.Threading.Tasks;

namespace BlockChainBackend.Services
{
    public interface IWalletService
    {
        Task<string> GetWalletAdress(string CustomerId);
        Task<string> CreateAndAddAdressToWallet(string CustomerId);
    }
}