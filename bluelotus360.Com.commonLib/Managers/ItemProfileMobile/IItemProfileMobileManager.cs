using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ItemProfileMobile
{
    public interface IItemProfileMobileManager : IManager
    {
        bool IsExceptionthrown();

        Task<IList<ItemSelectList>> GetItemProfileList(ItemSelectListRequest request); //getItemList function

        Task<ItemSelectList> InsertItem(ItemSelectList request);  //insertItem
        Task<ItemSelectList> UpdateItem(ItemSelectList request);  //UpdateItem
    }
}
