using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.Transaction
{
   public interface ITransactionValidator
    {
        UserMessageManager UserMessages { get; set; }

       
        bool CanAddItemToGrid(decimal? TransactionQuantiy=null);

        bool CanChangeHeaderInformatiom();

        bool CanItemCodeSearch();

        bool CanSaveTransaction();

        bool CanItemComboRequestFromServer();
    }
}
