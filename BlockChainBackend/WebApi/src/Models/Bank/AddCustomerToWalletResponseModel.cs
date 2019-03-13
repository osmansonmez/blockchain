namespace BlockChainBackend.Models.Bank
{
    public class AddCustomerToWalletResponseModel : BaseBankResponseViewModel
    {
        public AccountModel Account { get; set; }
    }
}