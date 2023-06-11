using BL10.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.Product
{
    public partial class ProductListView
    {
        [Parameter]
        public BLUIElement BLUIElement { get; set; }

        public IList<ItemRateResponse> Items { get; set; }
        public IList<ItemRateResponse> _filteredItem;
        public CodeBase Category { get; set; }

        private string filterQuery;

        private string SearchQuery = "";
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await ReadProducts();
        }

        private async Task ReadProducts()
        {
            if (BLUIElement != null && _orderState.SelectedCategory != null)
            {
                ItemRetrivalDTO req = new ItemRetrivalDTO();
                req.ObjectKey = BLUIElement.ElementKey;
                req.ItemCat7Key = _orderState.SelectedCategory.CodeKey;
                req.LocationKey = _orderState.SelectedLocation.CodeKey;
                req.FormObjectKey = _orderState.OrderPageElementKey;
                req.SearchQuery = SearchQuery;
                Items = await _comboManager.GetItemsByCatgory(req);
                _filteredItem = Items;
            }
        }


        public async Task OnFilterQueryChanged(string value)
        {
            filterQuery = value;
            if (filterQuery.Length > 2)
            {
                if (_orderState.SelectedCategory.IsCode10On)
                {
                    SearchQuery = filterQuery;
                    await ReadProducts();
                }
                else
                {
                    _filteredItem = Items.Where(x => x.RateItem.ItemName.Contains(filterQuery, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
			}
            else
            {

                _filteredItem = Items;


            }
        
            await Task.CompletedTask;
        }

    }
}
