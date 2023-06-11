using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs
{
    public partial class PriceListDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public EventCallback<UIInterectionArgs<string>> MatchItemByItemCode { get; set; }

        private bool Loading = false;
        string price_list_get_from_session;
        IEnumerable<PriceListResponse> price_list=new List<PriceListResponse>();

        private MudTable<PriceListResponse> mudTable;
        
        private int selectedRowNumber = -1;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Loading = true;
            price_list_get_from_session = await _sessionStorage.GetItemAsync<string>("data_connect");

            if (!string.IsNullOrEmpty(price_list_get_from_session))
            {
                price_list = JsonConvert.DeserializeObject<List<PriceListResponse>>(price_list_get_from_session);
            }

            price_list = FindByItemCode(SearchTerm);

            Loading = false;

            
        }

        public List<PriceListResponse> FindByItemCode(string searchTerm)
        {

            return price_list.Where(r => r.ItemCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).OrderByDescending(i => i.ItemCode).ToList();
        }

        private void RowClickEvent(TableRowClickEventArgs<PriceListResponse> tableRowClickEventArgs)
        {
            UIInterectionArgs<string> args = new UIInterectionArgs<string>();

            args.DataObject = tableRowClickEventArgs.Item.ItemCode;
            MatchItemByItemCode.InvokeAsync(args);

            MudDialog.Close(DialogResult.Ok(true));

            StateHasChanged();
   
        }

        private string SelectedRowClassFunc(PriceListResponse element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                
                return string.Empty;
            }
            else if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
                
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
