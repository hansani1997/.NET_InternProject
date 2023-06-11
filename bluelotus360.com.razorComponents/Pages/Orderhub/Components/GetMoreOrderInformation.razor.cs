using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Orderhub.Components
{
    public partial class GetMoreOrderInformation
    {
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; } = true;
        [Parameter] public IList<PartnerOrderDetails> _Order { get; set; }
        [Parameter] public PartnerOrder selectedOrder { get; set; }

        private bool ReportShown = false;
        //private TerlrikReportOptions _InvoiceReportOption;
        private decimal subTotal = 0;
        private decimal FinalTotal = 0;
        private DialogOptions dialogOptions = new() { CloseButton = true };

        CompletedUserAuth auth;

        public GetMoreOrderInformation()
        {
            _Order = new List<PartnerOrderDetails>();
            selectedOrder = new PartnerOrder();
        }

        protected override async Task OnInitializedAsync()
        {
            //_InvoiceReportOption = new TerlrikReportOptions();
            //_InvoiceReportOption.ReportParameters = new Dictionary<string, object>();
            auth = await _authenticationManager.GetUserInformation();
        }

        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }
        //private async void PrintInvoice()
        //{
        //    if (selectedOrder.PartnerOrderId > 1)
        //    {
        //        OnCloseClick();
        //        if (_InvoiceReportOption != null && _InvoiceReportOption.ReportParameters != null)
        //        {
        //            _InvoiceReportOption.ReportParameters.Clear();



        //            _InvoiceReportOption.ReportParameters.Add("CKy", auth.AuthenticatedCompany.CompanyKey);
        //            _InvoiceReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
        //            _InvoiceReportOption.ReportParameters.Add("OrdKy", selectedOrder.PartnerOrderId);




        //            _InvoiceReportOption.ReportName = "OrderHubInvoice.trdp";
        //            ReportShown = true;
        //        }
        //    }
        //    else
        //    {
        //        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
        //        _snackBar.Add("Error!.", Severity.Error);
        //    }
        //    StateHasChanged();

        //}

        public void GetSingleOrder(PartnerOrder Order)
        {
            selectedOrder = Order;
            _Order = selectedOrder.OrderItemDetails.Where(x => x.OrderItem.ItemType.CodeName != "SERVICE").ToList();


            PartnerOrderDetails deliveryCharge = selectedOrder.OrderItemDetails.Where(x => x.OrderItem.ItemType.CodeName == "SERVICE").FirstOrDefault();
            if (deliveryCharge != null)
            {
                selectedOrder.DeliveryCharges = deliveryCharge.TransactionPrice;
                subTotal = _Order.Sum(x => x.BaseTotalPrice);
                selectedOrder.TotalWithDiscount = subTotal - selectedOrder.DiscountAmount;
                FinalTotal = selectedOrder.TotalWithDiscount + selectedOrder.DeliveryCharges;
            }
            else
            {
                selectedOrder.DeliveryCharges = 0;
                subTotal = _Order.Sum(x => x.BaseTotalPrice);
                selectedOrder.TotalWithDiscount = subTotal - selectedOrder.DiscountAmount;
                FinalTotal = selectedOrder.TotalWithDiscount;
            }
        }


    }
}
