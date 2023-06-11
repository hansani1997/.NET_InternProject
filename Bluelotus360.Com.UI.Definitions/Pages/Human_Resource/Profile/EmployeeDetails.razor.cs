using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.Human_Resource.Profile
{
    public partial class EmployeeDetails
    {
        [Parameter]
        public EmployeeModel Employee { get; set; }
        [Parameter] public long ElementKey { get; set; }
        private bool isOpen;
        private IList<PaySlipDetails> paySlip;
        private TerlrikReportOptions _paySlipOption;
        private bool PaySlipShown = false;
        CompletedUserAuth auth;
        protected override async Task OnInitializedAsync()
        {
            paySlip = new List<PaySlipDetails>();

            auth = await _authenticationManager.GetUserInformation();
            _paySlipOption = new TerlrikReportOptions();
            _paySlipOption.ReportName = "PaySlip.trdp";
            _paySlipOption.ReportParameters = new Dictionary<string, object>();


            await base.OnInitializedAsync();
        }

        private async void ViewSlip(SalaryHistory row)
        {
            //paySlip = await _hrManager.GeneratePaySlip(row);
            _paySlipOption.ReportParameters.Clear();
            _paySlipOption.ReportName = "PaySlip.trdp";
            _paySlipOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
            _paySlipOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
            _paySlipOption.ReportParameters.Add("ObjKy", ElementKey);
            _paySlipOption.ReportParameters.Add("SalTypKy", row.SalTypKy);
            _paySlipOption.ReportParameters.Add("LocKy", 1);
            _paySlipOption.ReportParameters.Add("PrjKy", 1);
            _paySlipOption.ReportParameters.Add("SalDt", row.SalaryDt);
            _paySlipOption.ReportParameters.Add("EmpKy", row.EmpKy);
            _paySlipOption.ReportParameters.Add("BUKy", 1);
            _paySlipOption.ReportParameters.Add("SalPrcsGrpKy", row.SalPrcsGrpKy);

            PaySlipShown = true;

            StateHasChanged();
        }
    }
}
