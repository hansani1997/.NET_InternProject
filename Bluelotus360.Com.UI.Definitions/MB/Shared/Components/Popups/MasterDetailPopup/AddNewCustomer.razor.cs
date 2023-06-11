using BL10.CleanArchitecture.Application.Validators.MasterData;
using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup
{
    public partial class AddNewCustomer
    {
        [Parameter]
        public BLUIElement? InitiatorElement { get; set; }

        [Parameter]
        public EventCallback<AddressMaster> OnCustomerCreated { get; set; }

        AddressMaster? customer, customerValidation;
        private bool IsPopUpShown = false;
        IList<ServerMessage> Messages;
        ICustomerValidator validator;
        bool HideMinMax { get; set; } = false;
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public EventCallback OnCloseButtonClick { get; set; }

        long elementKey;

        protected override async Task OnParametersSetAsync() 
        {
            elementKey = 1;

            if (InitiatorElement != null) 
            {
                InitiatorElement.CssClass = "";
            }

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);

            Messages = new List<ServerMessage>();
            customer = new AddressMaster();
            customer.ElementKey = elementKey;
            validator = new CustomerValidator(customer);
            customerValidation = new AddressMaster();

            if (InitiatorElement != null && InitiatorElement.Children.Count == 0)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = InitiatorElement.ReferenceElementKey;
                InitiatorElement.Children = (await _navManger.GetMenuUIElement(formrequest)).Children;//get ui elements
                InteractionHelper helper = new InteractionHelper(this, InitiatorElement);//formdefinition has all form objects 
                InteractionLogics = helper.GenerateEventCallbacks();//
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }
        public void ShowPopUp()
        {
            IsPopUpShown = true;
            StateHasChanged();
        }
        private async void OnSave(UIInterectionArgs<object> args) 
        {
            if (validator.IsValidCustomer())
            {
                customerValidation = await _addressManager.CreateCustomerValidation(customer);

                if (customerValidation.HasError)
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add(customerValidation.Message, Severity.Success);
                }
                else
                {
                    await _addressManager.CreateCustomer(customer);
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("A New Customer has been  created Successfully", Severity.Success);
                }
            }
            IsPopUpShown = false;
            StateHasChanged();
        }
        private async void OnCancel(UIInterectionArgs<object> args) 
        {
            HidePopUp();
        }
        public void HidePopUp()
        {
            IsPopUpShown = false;
            StateHasChanged();
        }
        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }
            IsPopUpShown = false;
            StateHasChanged();
        }

        //private async void OnMakeClick(UIInterectionArgs<ItemCodeResponse> args) 
        //{
        //    this.StateHasChanged();
        //}
    }
}
