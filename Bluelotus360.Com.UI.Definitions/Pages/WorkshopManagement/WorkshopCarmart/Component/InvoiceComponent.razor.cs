using BL10.CleanArchitecture.Application.Validators.Transaction;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using static MudBlazor.Colors;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class InvoiceComponent
    {
        [Parameter] public EventCallback<int> Activate { get; set; }
        [Parameter] public BLUIElement UIScope { get; set; }
        [Parameter] public WorkOrder DataObject { get; set; }
        
        private IDictionary<string, EventCallback> InteractionLogic { get; set; }

        private IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        BLUIElement InvoiceSection;
        BLUIElement TotalAmountSection;
        BLUIElement InvoiceHeader;
        BLUIElement InvoiceDet;
        BLUIElement ReceiptSection;
        private TelerikGrid<OrderItem> GridRef1 = new TelerikGrid<OrderItem>();

        private long elementKey = 1;
        private BLTransaction transaction = new();
        private ITransactionValidator validator;
        private ValidationPopUp _refUserMessage=new ValidationPopUp();
        private GenericReciept _refgenericReciept;

        OrderItem material = new OrderItem();
        private bool IsButtonDisabled;
        private bool ReportShown = false;
        private TerlrikReportOptions _carmartReportOption;
        CompletedUserAuth auth;
        protected override async Task OnParametersSetAsync()
        {
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);

            if (UIScope != null)
            {
                InvoiceSection = new BLUIElement();
                ReceiptSection = new BLUIElement();
                TotalAmountSection = new BLUIElement();
                InvoiceHeader = new BLUIElement();

                ReceiptSection = SplitUIComponent("ReceiptSection");
                TotalAmountSection = SplitUIComponent("TotalAmountSection");
                InvoiceHeader = SplitUIComponent("InvoiceHeader");
                //InvoiceDet = SplitUIComponent("InvoiceDet");

                ReceiptSection.Children.Add(ReceiptSection);
                TotalAmountSection.Children.Add(TotalAmountSection);
                InvoiceHeader.Children.Add(InvoiceHeader);
                //InvoiceSection.Children.Add(InvoiceDet);

                InteractionLogic = new Dictionary<string, EventCallback>();
                if (ObjectHelpers == null)
                {
                    ObjectHelpers = new Dictionary<string, IBLUIOperationHelper>();
                }
                InteractionHelper helper = new InteractionHelper(this, UIScope);
                InteractionLogic = helper.GenerateEventCallbacks();

                await InitilizeFormDefinitions();
                IsButtonDisabled = DataObject.OrderStatus.Code.Equals("Closed");

                Telerik.Blazor.Components.GridState<OrderItem> desiredState = new Telerik.Blazor.Components.GridState<OrderItem>()
                {
                    GroupDescriptors = new List<GroupDescriptor>()
                        {
                            new GroupDescriptor()
                            {
                                Member = "IsServiceItem",
                                MemberType = typeof(bool)
                            },
                            
                        },
                    

                };
                if (desiredState!=null)
                {
                    await GridRef1?.SetState(desiredState);
                }

                transaction.Location = DataObject.OrderLocation;
                transaction.ContraAccountObjectKey = 194514;
                transaction.Account = new AccountResponse() {  AccountKey = DataObject.SelectedVehicle.RegisteredAccount.AccountKey,
                                                               AccountName = DataObject.SelectedVehicle.RegisteredAccount.AccountName };
                transaction.AccountObjectKey = 194517;
                transaction.PaymentTerm = DataObject.OrderPaymentTerm;
                transaction.Rep = DataObject.OrderRepAddress;
                transaction.BussinessUnit = new CodeBaseResponse() { CodeKey = DataObject.Cd1Ky };
                transaction.DiscountAmount = DataObject.OrderItems.Sum(x=>x.GetLineDiscount());

                await SetValue("Customer", transaction.Account.AccountName);
                await SetValue("ServiceAdvisor",transaction.Rep.AddressName);

                StateHasChanged();
            }
            await base.OnParametersSetAsync(); 
        }

        private async Task InitilizeFormDefinitions() 
        {
            if (transaction == null)
            {
                transaction = new BLTransaction();
            }
            else
            {
                transaction.InvoiceLineItems.Clear();
                transaction.Address = new AddressResponse();
                transaction.Account = new AccountResponse();
                transaction.Amount = 0;
                transaction.Amount2 = 0;
                transaction.Amount3 = 0;
                transaction.Amount4 = 0;
                transaction.Amount5 = 0;
                transaction.Amount6 = 0;
                transaction.SubTotal = 0;
                transaction.NetAmount = 0;
                transaction.DeliveryDate = DateTime.Now;
                transaction.TransactionKey = 1;
                transaction.IsPersisted = false;
                transaction.PaymentTerm = new CodeBaseResponse();
                transaction.Code1 = new CodeBaseResponse();
                transaction.Location = new CodeBaseResponse();
                transaction.ContraAccount = new AccountResponse();
                transaction.Rep = new AddressResponse();
                transaction.SerialNumber = new ItemSerialNumber();
                transaction.SerialNumber.SerialNumber = string.Empty;
                transaction.Code2 = new CodeBaseResponse();
                transaction.SelectedLineItem = new TransactionLineItem();
                transaction.CalculateTotals();
                _carmartReportOption=new TerlrikReportOptions();
                _carmartReportOption.ReportParameters = new Dictionary<string, object>();

            }

            validator = new WorkShopValidator(transaction);
            transaction.ElementKey = elementKey;

            transaction.InvoiceLineItems.Clear();
            await LoadListToTransaction(DataObject.WorkOrderMaterials);
            await LoadListToTransaction(DataObject.WorkOrderServices);

           // await SetValue("Cash", transaction.GetOrderTotalWithoutDiscounts());
            await SetValue("Discount", transaction.GetOrderDiscountTotal());
            await SetValue("TotalPayable", DataObject.GetOrderTotalWithDiscounts().ToString("N2"));
            auth = await _authenticationManager.GetUserInformation();
        }

        private BLUIElement SplitUIComponent(string domName)
        {
            BLUIElement parent = new BLUIElement();
            if (UIScope != null && !string.IsNullOrEmpty(domName))
            {
                parent = UIScope.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals(domName)).FirstOrDefault();

            }

            if (parent != null)
            {
                parent.Children = UIScope.Children.Where(x => x.ParentKey == parent.ElementKey).ToList();
            }

            return parent;
        }


        #region ui events

        private async void OnReceiptClick(UIInterectionArgs<object> args) 
        {
            transaction.InvoiceLineItems.Clear();
            await LoadListToTransaction(DataObject.WorkOrderMaterials);
            await LoadListToTransaction(DataObject.WorkOrderServices);

            transaction.CalculateTotals();
            transaction.InitilizeNewLineItem();

            if (transaction.TransactionKey > 10) {
                IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
                ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
                ParameterView values = ParameterView.FromDictionary(ParamDictionary);
                await _refgenericReciept.SetParametersAsync(values);
                _refgenericReciept.ShowPopUp();

                StateHasChanged();
                await Task.CompletedTask;
            }
            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Select a Transaction"
                );
                return;
            }
        }
        private async Task OnRecieptSavedSuccessfully() 
        {
            await ReadCurrentPayedTotal();
            transaction.CalculateCBalances();
            transaction.CalculateTotals();
            if (transaction.IsPersisted)
            {
                await SaveTransaction();
            }
            StateHasChanged();
        }
        private async Task ReadCurrentPayedTotal() 
        {
            //RecieptDetailRequest request = new RecieptDetailRequest();
            //request.ElementKey = transaction.ElementKey;
            //request.TransactionKey = transaction.TransactionKey;
            //RecviedAmountResponse response = await _transactionManager.GetTotalPayedAmount(request);
            //transaction.Amount5 = response.TotalPayedAmount;
            StateHasChanged();
        }
        private async Task OnRecipetClose()
        {

            await Task.CompletedTask;
        }
        //private async void OnTrNNoChange(UIInterectionArgs<string> args)
        //{
        //    transaction.TransactionNumber = args.DataObject;
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        private async void OnSalesChange(UIInterectionArgs<AccountResponse> args)
        {
            transaction.ContraAccount = args.DataObject;
            transaction.ContraAccountObjectKey = args.InitiatorObject.ElementKey;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.Location = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnTransactionDateChange(UIInterectionArgs<DateTime?> args)
        {
            transaction.TransactionDate =(DateTime) args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        //private async void OnTrnCustomerChange(UIInterectionArgs<AccountResponse> args)
        //{
        //    transaction.Account = args.DataObject;
        //    transaction.AccountObjectKey = args.InitiatorObject.ElementKey;
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        private async void OnTrnPayementTermChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.PaymentTerm = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }
        //private async void OnServiceAdvisorChange(UIInterectionArgs<AddressResponse> args)
        //{
        //    transaction.Rep = args.DataObject;
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //
        //private async void OnLineItemComboChange(UIInterectionArgs<ItemResponse> args)
        //{
        //    material.TransactionItem = args.DataObject;
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnTrnUnitChange(UIInterectionArgs<UnitResponse> args)
        //{
        //    material.TransactionUnit = args.DataObject;
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnLineTransactionNumercBoxChanged(UIInterectionArgs<decimal> args)
        //{

        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnLineTransactionTextBoxChanged(UIInterectionArgs<string> args)
        //{
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnLineItemAddToGrid(UIInterectionArgs<object> args)
        //{
        //    material.IsActive = 1;
        //    material.LineNumber = DataObject.WorkOrderMaterials.Count() + 1;
        //    DataObject.WorkOrderMaterials.Add(material);
        //    GridRef1?.Rebind();

        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}
        //private async void OnLineItemClear(UIInterectionArgs<object> args)
        //{
        //    StateHasChanged();
        //    await Task.CompletedTask;
        //}


        #region Total amout Section Event

        private async void OnDiscountChange(UIInterectionArgs<decimal> args) 
        {
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnCashChange(UIInterectionArgs<decimal> args)
        {
            decimal Total = args.DataObject - DataObject.GetOrderTotalWithDiscounts();
            await SetValue("Balance", Total.ToString("N2")); 
            await Task.CompletedTask;
        }
        private async void OnBalanceChange(UIInterectionArgs<decimal> args)
        {
            StateHasChanged();
            await Task.CompletedTask;
        }

        #endregion
        private async void OnAddServicesAndMaterials() 
        {
            if (Activate.HasDelegate)
            {
                await Activate.InvokeAsync(1);
                StateHasChanged();
            }
            StateHasChanged();
        }

        #endregion


        #region add material
        void EditHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
        }
        async Task UpdateHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = DataObject.OrderItems.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            if (index != -1)
            {
                DataObject.OrderItems[index] = item;
            }

            GridRef1?.Rebind();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }
        async Task DeleteHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = DataObject.OrderItems.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            DataObject.OrderItems[index].IsActive = 0;

            GridRef1?.Rebind();
            StateHasChanged();
            Console.WriteLine("Delete event is fired.");
        }
        #endregion


      

        #region Invoice Event

        private async void OnSaveClick() //check this one ,it is not completed
        {
            this.appStateService.IsLoaded = true;

            transaction.TransactionProject = DataObject.OrderProject;
            transaction.BussinessUnit = new CodeBaseResponse();//need to assign
            transaction.ElementKey = UIScope.ElementKey;
            transaction.Account = new AccountResponse();// need to assign 
            transaction.Address = new AddressResponse();
            transaction.ApproveState = new CodeBaseResponse();
            transaction.Rep = new AddressResponse();
            transaction.Code1 = new CodeBaseResponse(); //new CodeBaseResponse() { CodeKey = DataObject.Cd1Ky };
            transaction.IsActive = 1;
            transaction.IsApproved = 1;
            transaction.HeaderDiscountAmount = 0;//check
            transaction.DueDate= DateTime.Now;  //check it is correct or not
            transaction.DeliveryDate = DataObject.DeliveryDate;
            //service advisor where to assign

            transaction.InvoiceLineItems.Clear();
            await LoadListToTransaction(DataObject.WorkOrderMaterials);
            await LoadListToTransaction(DataObject.WorkOrderServices);
            await LoadListToTransaction(DataObject.OtherServices);

            transaction.CalculateTotals();

            await SaveTransaction();

            transaction.InitilizeNewLineItem();
            this.appStateService.IsLoaded = false; 
            StateHasChanged();
        }

        private async Task LoadListToTransaction(IList<OrderItem> orderItems) //check this one ,it is not completed
        {

            if (validator.CanAddItemToGrid()) 
            {
                foreach (OrderItem order in orderItems) 
                {
                    TransactionLineItem SelectedLineItem = new TransactionLineItem();
                    if (transaction.TransactionKey>1)
                    {
                        SelectedLineItem.IsPersisted=true;
                    }
                    SelectedLineItem.LineNumber = (int)order.LineNumber;
                    SelectedLineItem.TransactionItem = order.TransactionItem;
                    SelectedLineItem.TransactionLocation = order.OrderLineLocation;//check this value is assigning or not 
                    SelectedLineItem.Quantity= order.TransactionQuantity; 
                    SelectedLineItem.TransactionQuantity = order.TransactionQuantity;
                    SelectedLineItem.TransactionUnit = order.TransactionUnit;
                    SelectedLineItem.Rate = order.TransactionRate;
                    SelectedLineItem.TransactionRate = order.TransactionRate;
                    SelectedLineItem.DiscountAmount = order.DiscountAmount;
                    SelectedLineItem.TransactionDiscountAmount= order.DiscountAmount; 
                    SelectedLineItem.DiscountPercentage = order.DiscountPercentage;
                    SelectedLineItem.BussinessUnit = order.BussinessUnit;
                    SelectedLineItem.TransactionProject = DataObject.OrderProject;
                    SelectedLineItem.Address = DataObject.OrderCustomer;
                    SelectedLineItem.IsActive = order.IsActive; 
                    SelectedLineItem.IsApproved= order.IsApproved;
                    SelectedLineItem.Code1 = order.BussinessUnit;//check this value is assigning or not
                    SelectedLineItem.Description = "";
                    SelectedLineItem.HeaderDiscountAmount = 0;//check
                    SelectedLineItem.ItemTaxType1 = 0;
                    SelectedLineItem.ItemTaxType2 = 0;
                    SelectedLineItem.ItemTaxType3 = 0;
                    SelectedLineItem.ItemTaxType4 = 0;
                    SelectedLineItem.ItemTaxType5 = 0;
                    SelectedLineItem.ItemTaxType1Per = 0;
                    SelectedLineItem.ItemTaxType2Per = 0;
                    SelectedLineItem.ItemTaxType3Per = 0;
                    SelectedLineItem.ItemTaxType4Per = 0;
                    SelectedLineItem.ItemTaxType5Per = 0;
                    SelectedLineItem.DeliveryDate=DataObject.DeliveryDate;  
                    SelectedLineItem.SubTotal = order.SubTotal;
                    SelectedLineItem.PrincipleAmount = order.PrincipleAmount;
                    SelectedLineItem.PrinciplePrecentage = order.PrinciplePrecentage;
                    SelectedLineItem.CompanyPrecentage = order.CompanyAmount;
                    SelectedLineItem.CompanyPrecentage = order.CompanyPrecentage;
                    SelectedLineItem.CustomerAmount = order.CustomerAmount;


                    //technician,time,car per,principal per
                    transaction.InvoiceLineItems.Add(SelectedLineItem);
                }
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }
            StateHasChanged();
        }
        private async Task SaveTransaction() 
        {
            validator = new WorkShopValidator(transaction);
            if (validator.CanSaveTransaction())
            {
                await _workshopManager.SaveWorkOrderTransaction(transaction);

                //DataObject.OrderStatus = new CodeBaseResponse() { OurCode = "Closed" };
                //DataObject.FormObjectKey = UIScope.ElementKey;
                //await _workshopManager.EditWorkOrder(DataObject);

                
                _snackBar.Configuration.PositionClass = MudBlazor.Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invoice has been  Saved Successfully", MudBlazor.Severity.Success);

                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = transaction.TransactionKey;
                await LoadTransaction(request);
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }
            await Task.CompletedTask;
        }

        private async Task LoadTransaction(TransactionOpenRequest request) 
        {
            URLDefinitions urlDefinitions = new URLDefinitions();
            BLTransaction otransaction = await _workshopManager.OpenWorkOrderTransaction(request);
            otransaction.ElementKey = transaction.ElementKey;

            transaction.CopyFrom(otransaction);
            await ReadCurrentPayedTotal();

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = ObjectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }

            transaction.CalculateTotals();
        }
        private async void OnPrintClick()
        {
            if (transaction.TransactionKey > 1)
            {
                if (_carmartReportOption != null && _carmartReportOption.ReportParameters != null)
                {
                    _carmartReportOption.ReportParameters.Clear();
                    
                    _carmartReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _carmartReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    //_carmartReportOption.ReportParameters.Add("UsrId", auth.AuthenticatedUser.UserID);
                    _carmartReportOption.ReportParameters.Add("TrnKy", transaction.TransactionKey);
                    _carmartReportOption.ReportParameters.Add("ObjKy", UIScope.ElementKey);

                    if (DataObject.OrderCategory1.Code.Equals("Retail"))
                    {
                        _carmartReportOption.ReportName = "Invoice-Retail.trdp";
                    }
                    else if (DataObject.OrderCategory1.Code.Equals("Internal"))
                    {
                        _carmartReportOption.ReportName = "Invoice-Internal.trdp";
                    }
                    else if (DataObject.OrderCategory1.Code.Equals("Warranty"))
                    {
                        _carmartReportOption.ReportName = "Invoice-Warranty.trdp";
                    }
                    else if (DataObject.OrderCategory1.Code.Equals("Good Will Warranty"))
                    {
                        _carmartReportOption.ReportName = "Invoice-Good Will Warranty.trdp";
                    }
                    else if (DataObject.OrderCategory1.Code.Equals("Free"))
                    {
                        _carmartReportOption.ReportName = "Invoice-Free.trdp";
                    }
                    else
                    {

                    }


                }
                ReportShown = true;
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error!.", Severity.Error);
            }
            StateHasChanged();
        }

        #endregion

     

        #region object helpers

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
            }
        }

        private async Task Focus(string name)
        {
            IBLUIOperationHelper helper;

            if (ObjectHelpers.TryGetValue(name, out helper))
            {
                await helper.FocusComponentAsync();
                StateHasChanged();
            }
        }

        #endregion
    }
}
