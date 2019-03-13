namespace BlockChainBackend.Models
{
    public class BackendUserInfo
    {
        public string UserId { get; set; }
        
        public UserTypeEnum UserType { get; set; }
      
        public string Password { get; set; }
    }

    public enum UserTypeEnum
    {
        None = 0,
        Bank,
        Customer,
        OtherUsers
    }
}