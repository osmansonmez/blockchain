using System.Numerics;

namespace ContractInterface.Common
{
    public static class Constants
    {
        public static string GETH_RPC = "GethRPC";

        #region Defautl values for contracts

        public static string DEFAULT_TEST_ACCOUNT_ADDRESS = "0x791e65CD37dA61786F3944CA42e1Cc4bC882241e";//"0x3a8b73a6A99eF3696596419bEc74915Da6a53db9";
        public static string DEFAULT_TEST_ACCOUNT_PASSWORD = "";
        public static BigInteger DEFAULT_GAS = 4500000;
        public static BigInteger DEFAULT_GAS_PRICE = 150;
        public static BigInteger DEFAULT_VALUE = 0;

        #endregion

        #region Directories
        public static string CONTRACTS_DIR_NAME = "SmartContracts";
        public static string CONTRACT_ARTIFACTS_DIR_NAME = "contracts";
        public static string CONTRACT_ARTIFACT_FILE_NAME = "ContractArtifacts";
        #endregion
    
        #region Cache

        public static string CACHE_CONTRACT_LIST = "ContractList";
        public static string CACHE_ACCOUNT_LIST = "AccountList";
        #endregion
    }
}
