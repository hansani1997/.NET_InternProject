using bluelotus360.Com.commonLib.Managers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Services
{
    public class ItemService
    {
        private IComboDataManager _comboManager;

        public IComboDataManager ComboManager { get => _comboManager; set => _comboManager = value; }

        public async Task<ItemRateResponse> RequestItemRateForTransaction(BLTransaction transaction, TransactionLineItem lineItem)
        {
            ItemRateRequest request = new ItemRateRequest();
            request.LocationKey = BaseResponse.GetKeyValue(transaction.Location);
            request.AddressKey = BaseResponse.GetKeyValue(transaction.Address);
            request.AccountKey = BaseResponse.GetKeyValue(transaction.Account);

            request.ItemKey = lineItem.TransactionItem.ItemKey;
            request.EffectiveDate = transaction.TransactionDate;
            request.ConditionCode = "TrnTyp";
            request.ObjectKey = transaction.ElementKey;
            request.Code1Key = BaseResponse.GetKeyValue(transaction.Code1);
            request.Code2Key = BaseResponse.GetKeyValue(transaction.Code2);
            return (await ComboManager.GetRate(request));
        }

        //lnd
        public async Task<CodeBaseResponseExtended> GetItemAditionalCharges(CodeBaseResponse codeBaseResponse)
        {

            ComboRequestDTO requestDTO = new ComboRequestDTO();
            requestDTO.EntityKey = codeBaseResponse.CodeKey;
            return await _comboManager.GetCodeBaseResponseExtended(requestDTO);




        }
    }
}
