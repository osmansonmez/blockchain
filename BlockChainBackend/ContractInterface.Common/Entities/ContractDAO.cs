using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System;

using Nethereum.Contracts.CQS;
using Nethereum.Contracts;

namespace ContractInterface.Common.Entities
{
    public class ContractDAO: AccountDAO
    {
        /// <summary>
        /// Merkle root of blockchain
        /// </summary>
        /// <value></value>
        public string StorageHash{get; set;} //Merkle root

        /// <summary>
        /// Bytecode of contract
        /// </summary>
        /// <value></value>
        public string CodeHash{get; set;} //bytecode of contract
        
        /// <summary>
        /// Contract Abi
        /// </summary>
        /// <value></value>
        public string Abi{get; set;}

        /// <summary>
        /// Contract owner
        /// </summary>
        /// <value></value>
        public EOAHolderDAO Owner{get; set;}

        /// <summary>
        /// Stake holders
        /// </summary>
        /// <value></value>
        public List<EOAHolderDAO> Holders{get; set;}

        /// <summary>
        /// Mapping of representatives with withdrawal right.
        /// </summary>
        /// <value></value>
        public List<EOAAppointeeDAO> Allowance{get; set;}

        /// <summary>
        /// Price to mine deployment.
        /// </summary>
        /// <value></value>
        public string GasPrice{get; set;}

        /// <summary>
        /// Contract name must be unique.
        /// </summary>
        /// <value></value>
        public string Name{get; set;}

        /// <summary>
        /// Underlying Nethereum smart contract.
        /// </summary>
        /// <value></value>
        public Contract Contract{get; set;}

        /// <summary>
        /// Total token supply of this contract
        /// </summary>
        /// <value></value>
        public BigInteger TotalSupply{get; set;}

        /// <summary>
        /// True if contract is no longer in use.
        /// </summary>
        /// <value></value>
        public bool Expired{get; set;}

        /// <summary>
        /// Time in UTC
        /// </summary>
        /// <value></value>
        public DateTime CreatedDateTime{get; set;}


        #region Constructors

        public ContractDAO(){}
        public ContractDAO(Contract contract)
        {
            Address = contract.Address;
        }
        #endregion
    }
}