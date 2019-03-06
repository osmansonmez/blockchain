using System.Collections.Generic;

namespace ContractInterface.Common.Entities
{
    public class EOAAppointeeDAO: AccountDAO
    {
        public long Allowance{get; set;}
        public EOAHolderDAO Holder{get; set;}
    }
}