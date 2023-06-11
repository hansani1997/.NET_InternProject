using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MudBlazor;
using Microsoft.AspNetCore.Components.Web;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;


namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class BLPriceList : IDisposable
    {
        public IEnumerable<PriceListResponse> PriceListData { get; set; }
        public PriceListResponse SelectedListItem { get; set; }

        [Parameter]
        public EventCallback<PriceListRequest> OnBeforePriceListLoad { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<PriceListResponse>> OnAfterPriceListLoad { get; set; }

        [Parameter]
        public EventCallback OnPriceListClose { get; set; }

        [Parameter]
        public EventCallback<PriceListResponse> OnPriceListItemSelect { get; set; }

        [Parameter]
        public CodeBaseResponse SelectedPriceList { get; set; }


        private string SearchQuery = "";
        private string RateSearchQuery = "";
        private string SelectedPriceListName = "";
        private bool showAlert = false;
        private MudMessageBox mbox;

    
        private PriceListRequest request;

        private string PreviousValue = "";
     

        private MudTextField<string> _refSearchBox;


        private MudTable<PriceListResponse> _refTable;



        protected override async Task OnInitializedAsync()
        {
            request = new PriceListRequest();
            await this.ReadPriceList();        
            await base.OnInitializedAsync();


        }



        public void InitilizeKeyBooadShortCuts()
        {
         
        }


        private void MoveToNextRecord()
        {
            if (this._refTable.FilteredItems != null && _refTable.FilteredItems.Count() > 0)
            {

                int Totals = _refTable.GetFilteredItemsCount();


                int Offset = 1;
                if (Totals == 0)
                {
                    return;
                }
                else
                {
                    var index = _refTable.FilteredItems.ToList().IndexOf(SelectedListItem);
                    var CurrentPage = _refTable.CurrentPage;
                    var RowsPerPage = _refTable.RowsPerPage;
                    int PageMonitor = (CurrentPage + 1) * RowsPerPage;
                    if (index + Offset == PageMonitor)
                    {
                        _refTable.NavigateTo(_refTable.CurrentPage + 1);
                        _refTable.SetSelectedItem(this._refTable.FilteredItems.ElementAt(index + Offset));
                        return;


                    }
                    if (index < _refTable.FilteredItems.Count())
                    {
                        if (index < Totals - 1)
                        {
                            _refTable.SetSelectedItem(this._refTable.FilteredItems.ElementAt(index + Offset));

                        }
                    }

                }

            }
        }

        private void MoveToPreviousRecord()
        {
            if (this._refTable.FilteredItems != null && _refTable.FilteredItems.Count() > 0)
            {
                int Totals = _refTable.GetFilteredItemsCount();

                int Offset = -1;
                if (Totals == 0)
                {
                    return;
                }
                else
                {
                    var index = _refTable.FilteredItems.ToList().IndexOf(SelectedListItem);
                    if (index == 0)
                    {
                        return;
                    }
                    var CurrentPage = _refTable.CurrentPage;
                    var RowsPerPage = _refTable.RowsPerPage;
                    int PageMonitor = (CurrentPage) * RowsPerPage;
                    if (index == PageMonitor)
                    {
                        _refTable.NavigateTo(_refTable.CurrentPage - 1);
                        _refTable.SetSelectedItem(this._refTable.FilteredItems.ElementAt(index + Offset));
                        return;


                    }
                    if (index < _refTable.FilteredItems.Count())
                    {
                        if (index < Totals - 1)
                        {
                            _refTable.SetSelectedItem(this._refTable.FilteredItems.ElementAt(index + Offset));

                        }
                    }

                }

            }
        }


        private void OnEnterKeyPresss()
        {
            OnItemSelect(this.SelectedListItem);
        }




        public async Task ReadPriceList(bool forceServerRead = false)
        {
            if (OnBeforePriceListLoad.HasDelegate)
            {
                await OnBeforePriceListLoad.InvokeAsync(request);
            }
            if (request.PreviousKey < 11)
            {
                return;
            }
            string key = $"pl_{request.Code1Key}_{DateTime.Today.ToString("dd-MMM-yyyy")}_{request.PreviousKey}";
            var list = await _localStorage.GetItemAsync<IEnumerable<PriceListResponse>>(key);
            if (list == null || forceServerRead)
            {
                list = await _comboManager.GetPriceLists(request);
            }
            PriceListData = list;
            if (OnAfterPriceListLoad.HasDelegate)
            {
                await OnAfterPriceListLoad.InvokeAsync(list);
            }
            SelectFristRecord();



        }


        private void SelectFristRecord()
        {

            if (_refTable.FilteredItems != null && _refTable.FilteredItems.Count() > 0)
            {
                int offset = _refTable.CurrentPage * _refTable.RowsPerPage;
                _refTable.SetSelectedItem(_refTable.FilteredItems.ElementAt(offset));
            }
        }

        private bool FilterPriceList(PriceListResponse priceList)
        {
            if ((SearchQuery == null || SearchQuery.Trim().Length == 0) && (RateSearchQuery==null || RateSearchQuery.Trim().Length==0))
            {
                return true;
            }
            else if (priceList.ItemCode.Contains(SearchQuery, StringComparison.InvariantCultureIgnoreCase) || priceList.ItemName.Contains(SearchQuery, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;

            }

            return false;

        }

        private bool FilterPriceList(string SearchQuery)
        {
            return true;

        }

        protected override Task OnParametersSetAsync()
        {
            if (SelectedPriceList != null && SelectedPriceList.CodeKey > 10)
            {
                SelectedPriceListName = this.SelectedPriceList.CodeName;
            }
            else
            {
                SelectedPriceListName = "No Price List Selected";

            }
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_refSearchBox != null)
            {
                await _refSearchBox.FocusAsync();

            }

            await base.OnAfterRenderAsync(firstRender);
        }
        protected override void OnAfterRender(bool firstRender)
        {

            base.OnAfterRender(firstRender);
        }

        private void OnPriceListCloseButtonClick()
        {
           

            if (OnPriceListClose.HasDelegate)
            {
                OnPriceListClose.InvokeAsync();
            }
        }

        private async void RefreshPriceListClick()
        {
            await ReadPriceList(true);
        }

        public PriceListResponse FindExact(string Query)
        {

            if (Query == null || Query.Length == 0)
            {
                return null;
            }
            else
            {



                var priceListItem = PriceListData.Where(x => x.ItemCode.ToLower().Equals(Query.ToLower(), StringComparison.InvariantCultureIgnoreCase) ||
                 (x.EAN != null && x.EAN.ToLower().Equals(Query.ToLower()))
               );
                if(priceListItem!=null && priceListItem.Count() == 1)
                {
                   
                    return priceListItem.OrderByDescending(x=>x.LooseUnitPrice).FirstOrDefault();
                }
                else
                {
                    return priceListItem.OrderByDescending(x => x.LooseUnitPrice).FirstOrDefault();

                }

                return null;
            }


        }

        public void SetSearch(string Query)
        {
            SearchQuery = Query;            
            StateHasChanged();

        }

        public async Task FocusText()
        {
            if (_refSearchBox != null)
            {
                await _refSearchBox.FocusAsync();
                StateHasChanged();
            }
        
        }

        private string RowClassSelection(PriceListResponse response, int RowNumber)
        {
            string value = string.Empty;
            if (SelectedListItem != null && response.Equals(SelectedListItem))
            {
                value=  "selected";
            }

           

            return value;
        }

        public async void OnItemSelect(PriceListResponse item)
        {

            if (item.CanAddToTransaction())
            {
                if (this.OnPriceListItemSelect.HasDelegate)
                {
                    await OnPriceListItemSelect.InvokeAsync(item);

                }
                OnPriceListCloseButtonClick();

            }
            else
            {
                showAlert = true;
                StateHasChanged();
            }




        }


        private IEnumerable<PriceListResponse> GetPriceListResponses()
        {
            return this.PriceListData;
        }

        private async void OnSearchQueryChanged(string value)
        {
            SearchQuery = value;
            SelectFristRecord();
            await Task.CompletedTask;
        }
        private async void OnRateQuueryChanged(string value)
        {
            RateSearchQuery = value;
            SelectFristRecord();
            await Task.CompletedTask;
        }
        public void Dispose()
        {
           
        }


        
    }
}
