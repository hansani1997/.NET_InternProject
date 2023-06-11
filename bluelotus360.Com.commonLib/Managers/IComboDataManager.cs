using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Domain.DTO.Object;

namespace bluelotus360.Com.commonLib.Managers
{
    public interface IComboDataManager : IManager
    {
        Task<IList<CodeBaseResponse>> GetCodeBaseResponses(ComboRequestDTO requestDTO);

        Task<IList<AddressResponse>> GetAddressResponses(ComboRequestDTO requestDTO);

        Task<IList<ItemResponse>> GetItemResponses(ComboRequestDTO requestDTO);

        Task<IList<UnitResponse>> GetItemUnits(ComboRequestDTO requestDTO);

        Task<ItemRateResponse> GetRate(ItemRateRequest baseRequest);

        Task<IList<ItemCodeResponse>> GetItemByItemCode(ItemRequestModel itemRequest);


        Task<IList<PriceListResponse>> GetPriceLists(PriceListRequest price_list_request);

        Task<IList<AccountResponse>> GetAccountResponse(ComboRequestDTO requestDTO);

        Task<IList<AccPaymentMappingResponse>> GetPayementAccountMapping(AccPaymentMappingRequest response);

        Task<IList<BinaryDocument>> GetItemDocuments(ItemRequestModel response);

        Task<AddressCreateResponse> CreateNewAddress(AddressResponse response);

        Task<Base64Document> GetBase64TopDocument(DocumentRetrivaltDTO request);
        Task<IList<CodeBaseResponse>> GetNextApproveStatusResponses(ComboRequestDTO requestDTO);
        Task<IList<CodeBaseResponse>> GetApproveStatusResponses(ComboRequestDTO requestDTO);
        Task<CodeBaseResponseExtended> GetCodeBaseResponseExtended(ComboRequestDTO requestDTO);
        Task<IList<ItemSerialNumber>> GetSerialNumberResponses(ComboRequestDTO requestDTO);
        Task<IList<CodeBase>> ReadCategories(ComboRequestDTO requestDTO);
        Task<IList<ItemRateResponse>> GetItemsByCatgory(ItemRetrivalDTO request);
    }
}
