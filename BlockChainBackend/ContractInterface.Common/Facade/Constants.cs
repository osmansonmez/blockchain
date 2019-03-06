using System.Numerics;

namespace ContractInterface.Common
{
    public static class Constants
    {
        public static string GETH_RPC = "GethRPC";

        #region Defautl values for contracts
        public static string DEFAULT_TEST_ACCOUNT_ADDRESS = "0x3C919D9570D5e5a724FEBc6Ed8662E42Aeb94428";
        public static string DEFAULT_TEST_ACCOUNT_PASSWORD = "";
        public static string BALLOT_TOKEN_CONTRACT_NAME = "Ballot";
        public static BigInteger DEFAULT_GAS = 3000000;
        public static BigInteger DEFAULT_VALUE = 0;

        #endregion

        #region Directories
        public static string CONTRACTS_DIR_NAME = "SmartContracts";
        public static string CONTRACT_ARTIFACTS_DIR_NAME = "Contracts";
        public static string CONTRACT_ARTIFACT_FILE_NAME = "ContractArtifacts";
        #endregion
    
        #region Cache

        public static string CACHE_CONTRACT_LIST = "ContractList";
        public static string CACHE_ACCOUNT_LIST = "AccountList";
        #endregion
    }
}