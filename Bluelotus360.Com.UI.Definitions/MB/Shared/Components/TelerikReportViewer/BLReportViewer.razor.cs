
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Telerik.ReportViewer.Blazor;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikReportViewer
{
    public partial class BLReportViewer
    {
        [Parameter]
        public TerlrikReportOptions ReportSetting { get; set; }

        [Parameter]
        public EventCallback CloseReportDelegate { get; set; }

        private ReportSourceOptions SourceOptions;



        private ReportViewer __refReportViewer;

        public BLReportViewer()
        {
           
           
        }

        protected override Task OnParametersSetAsync()
        {
          
            SourceOptions = new ReportSourceOptions();
            SourceOptions.Report = ReportSetting.ReportName;
            SourceOptions.Parameters = ReportSetting.ReportParameters;
            return base.OnParametersSetAsync();
        }


        private void CloseWindow()
        {
            if (CloseReportDelegate.HasDelegate)
            {
                CloseReportDelegate.InvokeAsync();
            }
        }
    }
}
