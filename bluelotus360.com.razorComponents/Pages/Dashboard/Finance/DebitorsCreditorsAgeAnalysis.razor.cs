using ApexCharts;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Dashboard.Finance
{
    public partial class DebitorsCreditorsAgeAnalysis
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter] public IList<Debtors_Creditors_Age_Analysis_DT> SelectedData { get; set; }
        [Parameter] public FinanceRequestDTDetails TransactionRequest { get; set; } = new FinanceRequestDTDetails();
        [Parameter] public string Header { get; set; }
        [Parameter] public DateTime Date { get; set; }
        [Parameter] public long ObjKy { get; set; }

        private IList<Debtors_Creditors_Transaction_Details> transaction_details;
        private FinanceRequestDTDetails request;
        private ApexChart<Debtors_Creditors_Age_Analysis_DT> _chart;
        private bool fixed_header = true;
        private bool isVisible = false;
        string transaction_header = "";

        protected override async Task OnInitializedAsync()
        {
            isVisible = false;
            request = new();
            request.ElementKey = TransactionRequest.ElementKey;
            request.ToDate = TransactionRequest.ToDate;
            request.BusinessUnit = TransactionRequest.BusinessUnit;

            transaction_details = new List<Debtors_Creditors_Transaction_Details>();
            await base.OnInitializedAsync();

        }
        private void Back()
        {
            MudDialog?.Close();
        }

        private void BackToAccDetails()
        {
            isVisible = false;
            StateHasChanged();
        }

        private async Task LoadTransactionDetails(Debtors_Creditors_Age_Analysis_DT itm)
        {
            transaction_details.Clear();

            if (itm.AccKy != null) request.AccKy = itm.AccKy;
            request.ElementKey = ObjKy;
            request.ToDate = Date;
            request.DayS = itm.DayS;
            request.DayE = itm.DayE;
            request.AccType = itm.AccountType;
            if (request.AccKy != null)
            {
                transaction_details = await _dashboardManager.Get_Debtor_Creditor_DT_Transaction_Details(request);
                if (request.AccType.Equals("Debtor")) { transaction_header = "Debtors Account -" + itm.accnm + ":" + Header; }
                if (request.AccType.Equals("Creditor")) { transaction_header = "Creditors Account -" + itm.accnm + ":" + Header; }
                isVisible = true;
                StateHasChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Can't load data", Severity.Error);
            }


        }
    }
}
