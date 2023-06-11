using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BL10.CleanArchitecture.Domain.Entities.Financial;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.IdentityModel.Tokens;

namespace bluelotus360.com.razorComponents.Pages.FinancialManagement
{
    public partial class PaymentVoucher
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
       
        long elementKey;
        private BLUIElement __paymentVoucherBtnGroup;
        private BLUIElement __paymentVoucherHeader;
        private BLUIElement __paymentVoucherDetail;
        private BLUIElement __paymentVoucherGrid;
        BLTable<Payment> _mudGrid = new BLTable<Payment>();
        Payment _pettyCash =new Payment();
        IList<Payment> _paymentList=new List<Payment>();    
        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements

                if (formDefinition != null)
                {
                    __paymentVoucherGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("PaymentVoucherGrid")).FirstOrDefault();
                    if (__paymentVoucherGrid != null)
                    {
                        __paymentVoucherGrid.Children = formDefinition.Children.Where(x => x.ParentKey == __paymentVoucherGrid.ElementKey).ToList();
                    }

                    _interactionLogic = new Dictionary<string, EventCallback>();
                    _modalDefinitions = new Dictionary<string, BLUIElement>();
                    _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

                    HookInteractions();
                }

                
            }
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "Payment Voucher";
        }

        private void InitializeForm()
        {
            _pettyCash = new();
        }
        private void UIStateCganged()
        {
            StateHasChanged();
        }

        #region ui events

        private async void OnEditClick(UIInterectionArgs<object> args)
        {
            UIStateCganged();
        }
        private async void OnNewClick(UIInterectionArgs<object> args)
        {
            UIStateCganged();
        }

        private async void OnSaveClick(UIInterectionArgs<object> args)
        {
            UIStateCganged();
        }

        private async void OnPrintClick(UIInterectionArgs<object> args)
        {
            UIStateCganged();
        }

        private async void OnSearchClick(UIInterectionArgs<object> args)
        {
            UIStateCganged();
        }

        private async void OnTrnDateChange(UIInterectionArgs<DateTime?> args)
        {
            _pettyCash.PaymentDate = (DateTime)args.DataObject;
            UIStateCganged();
        }
        private async void OnHeadersection1TextBoxChange(UIInterectionArgs<string> args)
        {
            UIStateCganged();
        }
        private async void OnFromAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            _pettyCash.DebitAccount= args.DataObject;
            UIStateCganged();
        }

        private async void OnToAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            _pettyCash.CreditAccount = args.DataObject;
            UIStateCganged();
        }
        private async void OnAdvAnalysisChange(UIInterectionArgs<AddressResponse> args)
        {
            _pettyCash.PaymentAddress = args.DataObject;
            UIStateCganged();
        }
        private async void OnDetailSectionNumericBoxChange(UIInterectionArgs<decimal> args)
        {
            _pettyCash.PaymentAmount= args.DataObject;  
            UIStateCganged();
        }
        private async void OnAddToGridClick(UIInterectionArgs<object> args)
        {
            if (_pettyCash.IsInEditMode)
            {

            }
            else
            {
                _paymentList.Add(_pettyCash);
            }

            //_pettyCash.Clear();

            UIStateCganged();
        }

        #endregion
    }
}
