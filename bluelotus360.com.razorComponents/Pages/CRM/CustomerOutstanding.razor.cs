using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using bluelotus360.Com.commonLib.Helpers;
using System.ComponentModel.DataAnnotations;
using BL10.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BL10.CleanArchitecture.Domain.Entities.Validation;
using bluelotus360.Com.commonLib.Routes;

namespace bluelotus360.com.razorComponents.Pages.CRM
{
    public partial class CustomerOutstanding
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private CustomerOutStanding _outstandingCheck = new CustomerOutStanding();
        ApiServerResponse<CustomerOutStadingDetails> validateModel=new ApiServerResponse<CustomerOutStadingDetails>();
        private CustomerOutStadingDetails _outstandingDetails = new CustomerOutStadingDetails();

        BLUIElement section1= new BLUIElement(), section2 = new BLUIElement(), section3 = new BLUIElement();
        public BackForwardParams _backForward = new BackForwardParams()
        {
            BackButton = "Dashboard",
            HasBackButton = true,
            BackwardRoute= "/dashboard/finance_db_mobile",
            BackObjectKey= 178265,
            ForwardButton = "Geo Attendence",
            HasForwardButton = true,
            ForwardRoute = "/crm/dsr_geo_attendence",
            ForwardObjectKey = 203419
        };

        #region general 
        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            if (formDefinition != null)
            {
                formDefinition.IsMustElements = formDefinition.Children.Where(x => x.IsMust).Select(i => i._internalElementName).ToList();
                HookInteractions();
                this.BreakComponent();
                formDefinition.IsDebugMode = false;

                section1 = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("Section1_Customer_OutStanding")).FirstOrDefault();
                section2 = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("Section2_Customer_OutStanding")).FirstOrDefault();
                section3 = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("Section3_Customer_OutStanding")).FirstOrDefault();
            }
            
            InitializeForm();
        }
        private void InitializeForm()
        {
            _outstandingCheck = new CustomerOutStanding();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "Customer Outstanding";
        }
        private void BreakComponent()
        {
            if (formDefinition != null)
            {
                var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
                foreach (var child in formDefinition.Children)
                {
                    child.Children = childsHash[child.ElementKey].ToList();
                }
                BLUIElement form = formDefinition.Children.Where(x => x.ElementKey == formDefinition.ElementKey).FirstOrDefault();
                if (form != null)
                {
                    formDefinition = form;

                }
            }
        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #endregion

        #region form events

        private async void OnAddressCat3Change(UIInterectionArgs<CodeBaseResponse> args)
        {
            _outstandingCheck.AddressCategory3 = args.DataObject;
            await ReadData("Section1_Customer_OutStanding_Customer");
            StateHasChanged();
        }
        private async void Section1_Customer_OutStanding_Customer_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("AdrCat3Ky", _outstandingCheck.AddressCategory3.CodeKey);
            StateHasChanged();
        }

        private async void OnAddressChange(UIInterectionArgs<AddressResponse> args)
        {
            _outstandingCheck.Address = args.DataObject;
            StateHasChanged();
        }

        private async void OnLoadClick(UIInterectionArgs<object> args)
        {
            validateModel = await _transactionManager.GetCustomerOutStandingDetails(_outstandingCheck,BaseEndpoint.BaseURL+ "Transaction/getCustomerOutstanding");
            if (validateModel!=null && validateModel.ExecutionException==null)
            {
                _outstandingDetails = validateModel.Value; 
            }
            else
            {

            }
            StateHasChanged();
        }
        #endregion

        #region other events

        
        #endregion

        #region object helpers

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                UIStateChanged();
                await Task.CompletedTask;
            }
        }

        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }

        private void ToggleEditability(string name, bool isEnable)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(isEnable);
                UIStateChanged();
            }
        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }

        #endregion
    }
}


