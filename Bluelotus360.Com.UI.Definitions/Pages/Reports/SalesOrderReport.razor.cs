
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;

namespace BlueLotus360.Com.UI.Definitions.Pages.Reports
{
    public partial class SalesOrderReport
    {
        [Parameter]
        public Order ReportOrder { get; set; }
        [Parameter]
        public ReportCompanyDetailsResponse CompanyInofrmation { get; set; } = new();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
    }
}
