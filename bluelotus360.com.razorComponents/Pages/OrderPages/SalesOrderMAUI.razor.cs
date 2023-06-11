using BL10.CleanArchitecture.Domain.DTO.Object;
using BlueLotus.Com.Domain.Entity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components.Dialogs;
using bluelotus360.com.razorComponents.Pages.OrderPages.ComPonent;
using bluelotus360.com.razorComponents.StateManagement;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.OrderPages
{
	public partial class SalesOrderMAUI
	{
        private long elementKey = 1;
        private bool _isOpen = false;
        #region parameter        
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private BLUIElement __categoryPage;
        private BLUIElement __addressPage;
        private BLUIElement __itemSetting;
        private BLUIElement __defaultLocation;
        private BLUIElement __payementTerms;
        private string CurrentCaategory = "";

        private OrderBottomBar _refBottomBar;
        Color __buttonColor;
        int colval = 0;
        private IList<CodeBase> Categories;
        public double Value { get; set; } = 50;

        #endregion
        protected override async Task OnInitializedAsync()
        {
            __buttonColor = Color.Default;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                _orderState.OrderPageElementKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
                if (formDefinition != null && formDefinition.Children != null)
                {
                    __categoryPage = formDefinition.Children.Where(x => x.ElementName.Equals("__categoryPage")).FirstOrDefault();
                    __addressPage = formDefinition.Children.Where(x => x.ElementName.Equals("__customerAddress")).FirstOrDefault();
                    __itemSetting = formDefinition.Children.Where(x => x.ElementName.Equals("__itemRead")).FirstOrDefault();
                    __defaultLocation = formDefinition.Children.Where(x => x.ElementName.Equals("__defaultLocation")).FirstOrDefault();
                    __payementTerms = formDefinition.Children.Where(x => x.ElementName.Equals("__payementTerm")).FirstOrDefault();
                    long DefaultLocationKey;
                    if (long.TryParse(__defaultLocation.DefaultValue, out DefaultLocationKey))
                    {
                        _orderState.SelectedLocation = new CodeBaseResponse((int)DefaultLocationKey);
                    }
                    else
                    {
                        _orderState.SelectedLocation = new();
                    }
                    _orderState.OrderUIDefintion = formDefinition;
                    _orderState.CurrentOrder.FormObjectKey = elementKey;

                }
            }

            await base.OnInitializedAsync();
            await ReadServerData();
        }


        private async Task ReadServerData()
        {
            double TotalCategory = 1, CurrentIndex = 1;
            if (__categoryPage != null)
            {
                ComboRequestDTO dto = new ComboRequestDTO();
                dto.RequestingElementKey = __categoryPage.ElementKey;
                dto.RequestingURL = BaseEndpoint.BaseURL + __categoryPage.GetPathURL();
                Categories = await _comboManager.ReadCategories(dto);
                await Task.CompletedTask;

            }

            TotalCategory = Categories.Count;

            if (__payementTerms != null)
            {
                ComboRequestDTO dto = new ComboRequestDTO();
                dto.RequestingElementKey = __payementTerms.ElementKey;
                dto.RequestingURL = BaseEndpoint.BaseURL + __payementTerms.GetPathURL();
                _orderState.PayementTerms = await _comboManager.ReadCategories(dto);
                if (_orderState.PayementTerms != null && _orderState.PayementTerms.Count > 1)
                {
                    _orderState.CurrentOrder.OrderPaymentTerm = _orderState.PayementTerms[1];
                }
                await Task.CompletedTask;
            }

            foreach (var category in Categories)
            {
                Value = (CurrentIndex / TotalCategory) * 100;
                // await ReadProducts(category);
                CurrentIndex++;
                if (CurrentIndex % 3 == 0)
                {
                    CurrentCaategory = category.CodeName + " - " + Value.ToString("N0");
                    StateHasChanged();

                }
            }
            _orderState.DisplayMode = WindowDisplayMode.CategoryListView;
            StateHasChanged();
            _orderState.NotifyUIStateChange = new EventCallback(this, (Action)StateChangeNotification);
        }



        private async Task ReadProducts(CodeBaseResponse ItemCat7)
        {
            if (formDefinition != null && ItemCat7 != null)
            {
                ItemRetrivalDTO req = new ItemRetrivalDTO();
                req.ObjectKey = formDefinition.ElementKey;
                req.ItemCat7Key = ItemCat7.CodeKey;
                req.LocationKey = _orderState.SelectedLocation.CodeKey;
                req.FormObjectKey = _orderState.OrderPageElementKey;
                var item = await _comboManager.GetItemsByCatgory(req);

                foreach (var line in item)
                {
                    line.ItemCategory7 = ItemCat7;
                    _orderState.Items.Add(line);
                }

            }
        }




        public async Task OnCategorySelected(CodeBase category)
        {
            _orderState.SelectedCategory = category;
            _orderState.DisplayMode = WindowDisplayMode.CategoryView;
            await Task.CompletedTask;

        }

        void StateChangeNotification()
        {
            if (_refBottomBar != null)
            {
                _refBottomBar.Refresh();
            }
            StateHasChanged();
        }

        public async Task OnCategoryBackButtonClicked()
        {
            StateHasChanged();
        }


        public async Task ToggleOpen()
        {
            _isOpen = !_isOpen;
            await Task.CompletedTask;

        }

        public async Task ShowCustomerAddWindow()
        {

            var options = new DialogOptions { CloseOnEscapeKey = true };
            _dialogService.Show<AddNewAddressDialog>("Add New Customer", options);
            await Task.CompletedTask;
        }

        public async Task CancelCustomerSelection()
        {
            _orderState.InitilizeNewOrder();
            _orderState.IsCustomerSelected = false;
            _orderState.SelectedCustomer = null;
            _isOpen = false;
            await Task.CompletedTask;

        }

        public async Task ClearCustomer()
        {
            _orderState.IsCustomerSelected = false;
            _orderState.SelectedCustomer = null;

            if (_orderState.NotifyUIStateChange.HasDelegate)
            {
                await _orderState.NotifyUIStateChange.InvokeAsync();
            }
            await Task.CompletedTask;
        }
        public async Task ShowColors()
        {
            if (colval < 10)
            {
                colval++;
            }
            else
            {
                colval = 0;
            }

            __buttonColor = (Color)colval;
            await Task.CompletedTask;
        }


    }
}
