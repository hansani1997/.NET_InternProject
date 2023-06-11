using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Application.Validators.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups.MasterDetailPopup
{
    public partial class AddNewAddress
    {
        [Parameter]
        public BLUIElement InitiatorElement { get; set; }

        [Parameter]
        public EventCallback<AddressMaster> OnAddressCreated { get; set; }
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }

        AddressMaster addressMaster = new AddressMaster();
        private bool IsPopUpShown = false;
        IList<ServerMessage> Messages;
        IAddressMasterValidator validator;
        private DialogOptions dialogOptions = new() { FullScreen=true };
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }




        protected override async Task OnParametersSetAsync()
        {
            addressMaster = new AddressMaster();
            validator = new AddressMasterValidator(addressMaster);
            Messages = new List<ServerMessage>();
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


        public async void HidePopUp()
        {
            IsPopUpShown = false;
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }
            StateHasChanged();
        }


        private async void OnSaveAddressClick(UIInterectionArgs<object> args)
        {
            await SaveAddress();
            StateHasChanged();
        }

        private async Task SaveAddress()
        {
            if (validator.IsValidAddress())
            {
                AddressCreateServerResponse response = new AddressCreateServerResponse();
                addressMaster.IsActive = 1;
                response = await _addressManager.CreateNewAddress(addressMaster);
                if (response!=null && !response.IsAddressIDAvailable)
                {


                    validator.ValidationMessages.AddErrorMessage($"Address Id ${addressMaster.AddressID} Is taken");

                }
                else
                {
                    if (OnAddressCreated.HasDelegate)
                    {
                        await OnAddressCreated.InvokeAsync(response.Value);
                    }
                    HidePopUp();

                }
            }
            else
            {

            }
            StateHasChanged();
        }
        private void OnCancelClick(UIInterectionArgs<object> args)
        {
            HidePopUp();
        }



        private void OnAddressIdChanged(UIInterectionArgs<string> args)
        {
            string value = args.DataObject;
        }

        //private async void OnParentAccChange(UIInterectionArgs<AddressResponse> args)
        //{
        //    this.StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnAccTypChange(UIInterectionArgs<AddressResponse> args)
        //{
        //    this.StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnCreditLimitChange(UIInterectionArgs<decimal> args)
        //{
        //    this.StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnCreditPeriodChange(UIInterectionArgs<CodeBaseResponse> args)
        //{
        //    this.StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnCountryChange(UIInterectionArgs<string> args)
        //{
        //    this.StateHasChanged();
        //    await Task.CompletedTask;
        //}
    }
}
