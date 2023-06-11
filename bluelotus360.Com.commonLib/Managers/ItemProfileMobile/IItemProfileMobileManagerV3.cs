using BL10.CleanArchitecture.Domain.DTO;
using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus.Com.Domain.Entity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ItemProfileMobile
{
    public interface IItemProfileMobileManagerV3 : IManager
    {
        //Task<ItemSelectListV3> GetItemProfileListV3(ItemSelectListRequestV3 request);
        Task<Item> InsertItemV3(Item request);
        Task<string> GetItemServerfilterDetails(BaseServerFilterInfo request);
        Task<Item> SelectSingleItem(Item request);
        Task MultiUnitsInsert(ItemUnit request);
        Task MultiUnitsUpdate(ItemUnit request);
        Task<Item> UpdateItem(Item request);
        Task<ItemCombinations> GenerateCombinations(ItemCombinations request);
        Task<IList<Item>> GetCombinationItems(Item request);
        Task CreateComponents(ItemComponent request);
        Task UpdateItemComponent(ItemComponent request);
        Task DeleteItemComponent(ItemComponent request);
        Task<ItemComponent> GetSingleItemComponent(ItemComponent request);
        Task<IList<ItemComponent>> GetItemComponents(Item request);
    }
}
